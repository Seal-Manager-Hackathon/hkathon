using Hackathon.Application.Services.Lecturer.Assign;
using Hackathon.Application.Services.Lecturer.Event;
using Hackathon.Application.Services.Lecturer.Notification;
using Hackathon.Application.Services.Lecturer.RegisterTeam;
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
        return services;
    }
}
