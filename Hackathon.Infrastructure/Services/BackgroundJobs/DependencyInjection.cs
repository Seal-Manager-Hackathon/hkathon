using Hackathon.Infrastructure.Services.BackgroundJobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Hackathon.Infrastructure;

public static class BackgroundJobRegistration
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            // ── Auto-close expired events ──
            var autoCloseEventsJobKey = new JobKey("AutoCloseExpiredEventsJob");
            q.AddJob<AutoCloseExpiredEventsJob>(opts => opts.WithIdentity(autoCloseEventsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(autoCloseEventsJobKey)
                .WithSimpleSchedule(s => s
                    .WithIntervalInMinutes(10)
                    .RepeatForever()));

            // ── Expire pending invitations ──
            var expireInvitationsJobKey = new JobKey("ExpirePendingInvitationsJob");
            q.AddJob<ExpirePendingInvitationsJob>(opts => opts.WithIdentity(expireInvitationsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(expireInvitationsJobKey)
                .WithSimpleSchedule(s => s
                    .WithIntervalInMinutes(10)
                    .RepeatForever()));

            // ── Expire pending email verifications ──
            var expireEmailVerificationsJobKey = new JobKey("ExpireEmailVerificationsJob");
            q.AddJob<ExpireEmailVerificationsJob>(opts => opts.WithIdentity(expireEmailVerificationsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(expireEmailVerificationsJobKey)
                .WithSimpleSchedule(s => s
                    .WithIntervalInMinutes(10)
                    .RepeatForever()));

            // ── Reject pending register teams past deadline ──
            var rejectPendingRegisterTeamsJobKey = new JobKey("RejectPendingRegisterTeamsJob");
            q.AddJob<RejectPendingRegisterTeamsJob>(opts => opts.WithIdentity(rejectPendingRegisterTeamsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(rejectPendingRegisterTeamsJobKey)
                .WithSimpleSchedule(s => s
                    .WithIntervalInMinutes(10)
                    .RepeatForever()));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
