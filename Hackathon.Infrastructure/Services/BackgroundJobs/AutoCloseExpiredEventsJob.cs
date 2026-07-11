using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Infrastructure.Services.BackgroundJobs;

[DisallowConcurrentExecution]
public class AutoCloseExpiredEventsJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AutoCloseExpiredEventsJob> _logger;

    public AutoCloseExpiredEventsJob(
        AppDbContext dbContext,
        ILogger<AutoCloseExpiredEventsJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTimeOffset.UtcNow;
        var cancellationToken = context.CancellationToken;

        var expiredEvents = await _dbContext.Events
            .Where(e => !e.IsDisable
                && e.Status == EventStatusEnum.Published
                && e.EndTime.HasValue
                && e.EndTime.Value <= now)
            .ToListAsync(cancellationToken);

        if (expiredEvents.Count == 0)
        {
            _logger.LogInformation("AutoCloseExpiredEventsJob: no expired events found.");
            return;
        }

        foreach (var ev in expiredEvents)
        {
            ev.Status = EventStatusEnum.Closed;
            ev.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "AutoCloseExpiredEventsJob: closed {Count} events whose EndTime has passed.",
            expiredEvents.Count);
    }
}
