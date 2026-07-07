using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Services.Auth;
using Hackathon.Application.Services.Event;
using Hackathon.Application.Services.Report;
using Hackathon.Application.Services.Team;
using Hackathon.Application.Services.User;
using Hackathon.Application.Services.Notification;
using Hackathon.Application.Services.RegisterTeam;
using Hackathon.Application.Services.Round;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, Services.Auth.Service>();
        services.AddScoped<IUserService, Services.User.Service>();
        services.AddScoped<IEventService, Services.Event.Service>();
        services.AddScoped<ITeamService, Services.Team.Service>();
        services.AddScoped<IReportService, Services.Report.Service>();
        services.AddScoped<IAuthorizationService, Services.Auth.AuthorizationService>();
        services.AddScoped<INotificationService, Services.Notification.Service>();
        services.AddScoped<IRegisterTeamService, Services.RegisterTeam.Service>();
        services.AddScoped<IRoundService, Services.Round.Service>();

        return services;
    }
}
