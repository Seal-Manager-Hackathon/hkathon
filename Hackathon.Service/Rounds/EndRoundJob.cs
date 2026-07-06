using System.Threading.Channels;
using Hackathon.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon.Service.Rounds;

public interface IRoundEndScheduler
{
    void ScheduleEvent(Guid eventId);
}

public class EndRoundJob : BackgroundService, IRoundEndScheduler
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EndRoundJob> _logger;
    private readonly Channel<Guid> _eventChannel = Channel.CreateUnbounded<Guid>();
    private readonly HashSet<Guid> _monitoredEvents = [];
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(5);

    public EndRoundJob(IServiceScopeFactory scopeFactory, ILogger<EndRoundJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void ScheduleEvent(Guid eventId)
    {
        _eventChannel.Writer.TryWrite(eventId);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EndRoundJob started.");

        var reader = _eventChannel.Reader;

        while (!stoppingToken.IsCancellationRequested)
        {
            while (reader.TryRead(out var newEventId))
            {
                lock (_monitoredEvents)
                {
                    _monitoredEvents.Add(newEventId);
                    _logger.LogInformation("EndRoundJob: Now monitoring event {EventId}.", newEventId);
                }
            }

            List<Guid> eventsToCheck;
            lock (_monitoredEvents)
            {
                eventsToCheck = [.. _monitoredEvents];
            }

            if (eventsToCheck.Count != 0)
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var now = DateTimeOffset.UtcNow;

                foreach (var eventId in eventsToCheck)
                {
                    try
                    {
                        var eventEntity = await dbContext.Events
                            .AsNoTracking()
                            .Where(x => x.Id == eventId && !x.IsDisable)
                            .Select(x => new { x.EndTime })
                            .FirstOrDefaultAsync(stoppingToken);

                        if (eventEntity == null)
                        {
                            lock (_monitoredEvents) { _monitoredEvents.Remove(eventId); }
                            _logger.LogInformation("EndRoundJob: Stopped monitoring event {EventId} — deleted/disabled.", eventId);
                            continue;
                        }

                        if (eventEntity.EndTime.HasValue && now > eventEntity.EndTime.Value)
                        {
                            await CloseExpiredRounds(dbContext, eventId, now, stoppingToken);
                            lock (_monitoredEvents) { _monitoredEvents.Remove(eventId); }
                            _logger.LogInformation("EndRoundJob: Stopped monitoring event {EventId} — event ended.", eventId);
                            continue;
                        }

                        await CloseExpiredRounds(dbContext, eventId, now, stoppingToken);
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        _logger.LogError(ex, "EndRoundJob: Error checking event {EventId}.", eventId);
                    }
                }
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private static async Task CloseExpiredRounds(AppDbContext dbContext, Guid eventId, DateTimeOffset now, CancellationToken ct)
    {
        var expiredRounds = await dbContext.Rounds
            .Where(x => x.EventId == eventId && !x.IsDisable && x.EndTime != null && x.EndTime < now)
            .ToListAsync(ct);

        if (expiredRounds.Count == 0)
            return;

        foreach (var round in expiredRounds)
        {
            try
            {
                await Service.CloseAndAdvanceRoundAsync(dbContext, round, now);
            }
            catch (Exception ex)
            {
                // Logged inside CloseAndAdvanceRoundAsync — re-throw would stop the loop
            }
        }
    }
}
