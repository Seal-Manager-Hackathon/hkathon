using Hackathon.Application.Services.Lecturer.Event;
using Hackathon.Application.Services.Lecturer.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Lecturer;

public static class DependencyInjection
{
    public static IServiceCollection AddLecturerServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        services.AddScoped<INotificationService, Notification.Service>();
        return services;
    }
}
