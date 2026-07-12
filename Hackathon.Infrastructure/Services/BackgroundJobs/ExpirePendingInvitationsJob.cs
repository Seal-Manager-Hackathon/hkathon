using Hackathon.Domain.Enums.Invitation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Infrastructure.Services.BackgroundJobs;

[DisallowConcurrentExecution]
public class ExpirePendingInvitationsJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ExpirePendingInvitationsJob> _logger;

    public ExpirePendingInvitationsJob(
        AppDbContext dbContext,
        ILogger<ExpirePendingInvitationsJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTimeOffset.UtcNow;
        var cancellationToken = context.CancellationToken;

        var expiredInvitations = await _dbContext.Invitations
            .Where(i => i.Status == InvitationStatusEnum.Pending
                && i.LimitTime.HasValue
                && i.LimitTime.Value <= now)
            .ToListAsync(cancellationToken);

        if (expiredInvitations.Count == 0)
        {
            _logger.LogInformation("ExpirePendingInvitationsJob: no expired invitations found.");
            return;
        }

        foreach (var inv in expiredInvitations)
        {
            inv.Status = InvitationStatusEnum.Expired;
            inv.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "ExpirePendingInvitationsJob: expired {Count} invitations whose LimitTime has passed.",
            expiredInvitations.Count);
    }
}
