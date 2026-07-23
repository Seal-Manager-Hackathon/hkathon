using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Script;

public static class DependencyInjection
{
    public static IServiceCollection AddScriptServices(this IServiceCollection services)
    {
        services.AddScoped<IScriptService, Service>();
        return services;
    }
}
