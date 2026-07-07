using Hackathon.Application.Services.Admin;
using Hackathon.Application.Services.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAdminServices();
        services.AddBaseServices();
        return services;
    }
}
