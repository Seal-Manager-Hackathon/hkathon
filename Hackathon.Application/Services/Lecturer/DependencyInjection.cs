using Hackathon.Application.Services.Lecturer.Event;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Lecturer;

public static class DependencyInjection
{
    public static IServiceCollection AddLecturerServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        return services;
    }
}
