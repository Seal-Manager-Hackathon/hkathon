using Hackathon.Domain.Enums.RegisterTeam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Infrastructure.Services.BackgroundJobs;

[DisallowConcurrentExecution]
public class RejectPendingRegisterTeamsJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<RejectPendingRegisterTeamsJob> _logger;

    public RejectPendingRegisterTeamsJob(
        AppDbContext dbContext,
        ILogger<RejectPendingRegisterTeamsJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTimeOffset.UtcNow;
        var cancellationToken = context.CancellationToken;

        // Lấy danh sách Event đã quá hạn đăng ký
        var expiredEventIds = await _dbContext.Events
            .AsNoTracking()
            .Where(e => !e.IsDisable
                && e.Status == Domain.Enums.Event.EventStatusEnum.Published
                && e.RegisterLimitTime.HasValue
                && e.RegisterLimitTime.Value <= now)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        if (expiredEventIds.Count == 0)
        {
            _logger.LogInformation("RejectPendingRegisterTeamsJob: no expired register limit events found.");
            return;
        }

        // Chỉ xử lý các RegisterTeam có Status = Pending
        var pendingRegistrations = await _dbContext.RegisterTeams
            .Where(rt => !rt.IsDisable
                && rt.Status == RegisterTeamStatusEnum.Pending
                && expiredEventIds.Contains(rt.EventId))
            .ToListAsync(cancellationToken);

        if (pendingRegistrations.Count == 0)
        {
            _logger.LogInformation("RejectPendingRegisterTeamsJob: no pending registrations to reject.");
            return;
        }

        foreach (var reg in pendingRegistrations)
        {
            reg.Status = RegisterTeamStatusEnum.Rejected;
            reg.RejectionReason = "Registration Deadline Has Passed";
            reg.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "RejectPendingRegisterTeamsJob: auto-rejected {Count} pending registrations whose events' RegisterLimitTime has passed.",
            pendingRegistrations.Count);
    }
}
