using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Judge;

public static class DependencyInjection
{
    public static IServiceCollection AddJudgeServices(this IServiceCollection services)
    {
        services.AddScoped<IJudgeService, Service>();
        return services;
    }
}
