using Hackathon.Application.Services.Base.Auth;
using Hackathon.Application.Services.Base.Award;
using Hackathon.Application.Services.Base.CriteriaTemplate;
using Hackathon.Application.Services.Base.Notification;
using Hackathon.Application.Services.Base.RegisterTeam;
using Hackathon.Application.Services.Base.Round;
using Hackathon.Application.Services.Base.Team;
using Hackathon.Application.Services.Base.Topic;
using Hackathon.Application.Services.Base.Track;
using Hackathon.Application.Services.Base.User;
using Hackathon.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Base;

public static class DependencyInjection
{
    public static IServiceCollection AddBaseServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, Auth.Service>();
        services.AddScoped<IAuthorizationService, Auth.AuthorizationService>();
        services.AddScoped<IUserProfileService, User.Service>();
        services.AddScoped<IRegisterTeamRoundService, RegisterTeam.Service>();
        services.AddScoped<INotificationService, Notification.Service>();
        services.AddScoped<ITopicService, Topic.Service>();
        services.AddScoped<ITrackService, Track.Service>();
        services.AddScoped<ITeamService, Team.Service>();
        services.AddScoped<IRoundService, Round.Service>();
        services.AddScoped<IAwardService, Award.Service>();
        services.AddScoped<ICriteriaTemplateService, CriteriaTemplate.Service>();
        return services;
    }
}
