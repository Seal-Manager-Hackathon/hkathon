using Hackathon.Application.Services.Student.Assign;
using Hackathon.Application.Services.Student.Award;
using Hackathon.Application.Services.Student.CriteriaTemplate;
using Hackathon.Application.Services.Student.Event;
using Hackathon.Application.Services.Student.Invitation;
using Hackathon.Application.Services.Student.Notification;
using Hackathon.Application.Services.Student.RegisterTeam;
using Hackathon.Application.Services.Student.Round;
using Hackathon.Application.Services.Student.Team;
using Hackathon.Application.Services.Student.Topic;
using Hackathon.Application.Services.Student.Track;
using Hackathon.Application.Services.Student.Leaderboard;
using Hackathon.Application.Services.Student.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Student;

public static class DependencyInjection
{
    public static IServiceCollection AddStudentServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        services.AddScoped<IAssignService, Assign.Service>();
        services.AddScoped<IAwardService, Award.Service>();
        services.AddScoped<IInvitationService, Invitation.Service>();
        services.AddScoped<ITeamService, Team.Service>();
        services.AddScoped<IUserService, User.Service>();
        services.AddScoped<ILeaderboardService, Leaderboard.Service>();
        services.AddScoped<INotificationService, Notification.Service>();
        return services;
    }
}
