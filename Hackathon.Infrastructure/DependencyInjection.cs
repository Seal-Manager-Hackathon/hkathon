using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Infrastructure.Services.Cloudinary;
using Hackathon.Infrastructure.Services.Jwt;
using Hackathon.Infrastructure.Services.Mail;
using Hackathon.Infrastructure.Services.Password;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Hackathon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddScoped<IJwtService, Services.Jwt.Service>();

        services.Configure<MailOptions>(configuration.GetSection("MailOptions"));
        services.AddScoped<IMailService, Services.Mail.Service>();

        services.Configure<CloudinaryOptions>(configuration.GetSection("CloudinaryOptions"));
        services.AddScoped<IMediaService, Services.Cloudinary.Service>();

        services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));
        services.AddScoped<IPasswordService, Services.Password.Service>();

        services.AddScoped<IUserRepository, Repositories.UserRepository>();
        services.AddScoped<IEmailVerificationRepository, Repositories.EmailVerificationRepository>();
        services.AddScoped<IRefreshTokenRepository, Repositories.RefreshTokenRepository>();
        services.AddScoped<IEventRepository, Repositories.EventRepository>();
        services.AddScoped<ITeamRepository, Repositories.TeamRepository>();
        services.AddScoped<IReportRepository, Repositories.ReportRepository>();
        services.AddScoped<INotificationRepository, Repositories.NotificationRepository>();
        services.AddScoped<IRegisterTeamRepository, Repositories.RegisterTeamRepository>();
        services.AddScoped<IRoundRepository, Repositories.RoundRepository>();
        services.AddScoped<IResetPasswordRepository, Repositories.ResetPasswordRepository>();
        services.AddScoped<ITrackRepository, Repositories.TrackRepository>();
        services.AddScoped<ITopicRepository, Repositories.TopicRepository>();
        services.AddScoped<ICriteriaTemplateRepository, Repositories.CriteriaTemplateRepository>();
        services.AddScoped<ICriteriaItemRepository, Repositories.CriteriaItemRepository>();
        services.AddScoped<IAwardRepository, Repositories.AwardRepository>();
        services.AddScoped<IAssignEventRepository, Repositories.AssignEventRepository>();
        services.AddScoped<ISubmissionRepository, Repositories.SubmissionRepository>();
        services.AddScoped<IScoreRepository, Repositories.ScoreRepository>();
        services.AddScoped<IMentorNotificationRepository, Repositories.MentorNotificationRepository>();
        services.AddScoped<IInvitationRepository, Repositories.InvitationRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, Services.CurrentUserService>();

        // Background Jobs — Quartz
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("AutoCloseExpiredEventsJob");
            q.AddJob<Services.BackgroundJobs.AutoCloseExpiredEventsJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithCronSchedule("0 */10 * * * ?")); // every 10 minutes

            var expireInvitationsJobKey = new JobKey("ExpirePendingInvitationsJob");
            q.AddJob<Services.BackgroundJobs.ExpirePendingInvitationsJob>(opts => opts.WithIdentity(expireInvitationsJobKey));
            q.AddTrigger(opts => opts
                .ForJob(expireInvitationsJobKey)
                .WithCronSchedule("0 */15 * * * ?")); // every 15 minutes
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
