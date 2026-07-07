using Hackathon.Application.Services.Admin.Award;
using Hackathon.Application.Services.Admin.Event;
using Hackathon.Application.Services.Admin.Notification;
using Hackathon.Application.Services.Admin.RegisterTeam;
using Hackathon.Application.Services.Admin.Report;
using Hackathon.Application.Services.Admin.Round;
using Hackathon.Application.Services.Admin.Team;
using Hackathon.Application.Services.Admin.Track;
using Hackathon.Application.Services.Admin.Topic;
using Hackathon.Application.Services.Admin.CriteriaTemplate;
using Hackathon.Application.Services.Admin.Assign;
using Hackathon.Application.Services.Admin.Leaderboard;
using Hackathon.Application.Services.Admin.Score;
using Hackathon.Application.Services.Admin.Submission;
using Hackathon.Application.Services.Admin.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Admin;

public static class DependencyInjection
{
    public static IServiceCollection AddAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IAwardService, Award.Service>();
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<INotificationService, Notification.Service>();
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        services.AddScoped<IReportService, Report.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<ITeamService, Team.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        services.AddScoped<IUserService, User.Service>();
        services.AddScoped<IAssignService, Assign.Service>();
        services.AddScoped<ISubmissionService, Submission.Service>();
        services.AddScoped<IScoreService, Score.Service>();
        services.AddScoped<ILeaderboardService, Leaderboard.Service>();
        return services;
    }
}
