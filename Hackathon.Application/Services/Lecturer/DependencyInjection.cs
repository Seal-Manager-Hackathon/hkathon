using Hackathon.Application.Services.Lecturer.Assign;
using Hackathon.Application.Services.Lecturer.Award;
using Hackathon.Application.Services.Lecturer.CriteriaTemplate;
using Hackathon.Application.Services.Lecturer.Event;
using Hackathon.Application.Services.Lecturer.Leaderboard;
using Hackathon.Application.Services.Lecturer.Round;
using Hackathon.Application.Services.Lecturer.Notification;
using Hackathon.Application.Services.Lecturer.RegisterTeam;
using Hackathon.Application.Services.Lecturer.Team;
using Hackathon.Application.Services.Lecturer.Topic;
using Hackathon.Application.Services.Lecturer.Track;
using Hackathon.Application.Services.Lecturer.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Lecturer;

public static class DependencyInjection
{
    public static IServiceCollection AddLecturerServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<INotificationService, Notification.Service>();
        services.AddScoped<IAssignService, Assign.Service>();
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        services.AddScoped<ITeamService, Team.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        services.AddScoped<IAwardService, Award.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<ILeaderboardService, Leaderboard.Service>();
        services.AddScoped<IUserService, User.Service>();
        return services;
    }
}
