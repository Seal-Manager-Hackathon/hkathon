using Hackathon.Application.Services.Staff.Event;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Staff;

public static class DependencyInjection
{
    public static IServiceCollection AddStaffServices(this IServiceCollection services)
    {
        services.AddScoped<IEventService, Event.Service>();
        return services;
    }
}
