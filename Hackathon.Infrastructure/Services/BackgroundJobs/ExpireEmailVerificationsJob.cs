using Hackathon.Domain.Enums.EmailVerification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Infrastructure.Services.BackgroundJobs;

[DisallowConcurrentExecution]
public class ExpireEmailVerificationsJob : IJob
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ExpireEmailVerificationsJob> _logger;

    public ExpireEmailVerificationsJob(
        AppDbContext dbContext,
        ILogger<ExpireEmailVerificationsJob> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTimeOffset.UtcNow;
        var cancellationToken = context.CancellationToken;

        var expiredVerifications = await _dbContext.EmailVerifications
            .Where(e => !e.IsDisable
                && e.Status == EmailVerificationStatusEnum.Pending
                && e.ExpiredAt <= now)
            .ToListAsync(cancellationToken);

        if (expiredVerifications.Count == 0)
        {
            _logger.LogInformation("ExpireEmailVerificationsJob: no expired email verifications found.");
            return;
        }

        foreach (var verification in expiredVerifications)
        {
            verification.Status = EmailVerificationStatusEnum.Expired;
            verification.UpdatedAt = now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "ExpireEmailVerificationsJob: expired {Count} pending email verifications whose ExpiredAt has passed.",
            expiredVerifications.Count);
    }
}
