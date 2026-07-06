using Hackathon.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, Services.Auth.Service>();

        return services;
    }
}
