using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Service.BackgroundJobService;

[DisallowConcurrentExecution]
public class ExpirePendingEmailVerificationsJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ExpirePendingEmailVerificationsJob> _logger;

    public ExpirePendingEmailVerificationsJob(
        AppDbContext dbContext,
        ILogger<ExpirePendingEmailVerificationsJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTimeOffset.UtcNow;

        var expiredEmailVerifications = await _dbContext.EmailVerifications
            .Where(x =>
                !x.IsDisable &&
                x.Status == EmailVerificationStatusEnum.Pending &&
                x.ExpiredAt <= now)
            .ToListAsync(context.CancellationToken);

        if (expiredEmailVerifications.Count == 0)
        {
            _logger.LogInformation("ExpirePendingEmailVerificationsJob: no expired email verifications found.");
            return;
        }

        foreach (var emailVerification in expiredEmailVerifications)
        {
            emailVerification.Status = EmailVerificationStatusEnum.Expired;
            emailVerification.UpdatedAt = now;
        }

        _dbContext.EmailVerifications.UpdateRange(expiredEmailVerifications);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation(
            "ExpirePendingEmailVerificationsJob: expired {Count} pending email verifications.",
            expiredEmailVerifications.Count);
    }
}
