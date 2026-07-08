using Hackathon.Application.Services.Admin;
using Hackathon.Application.Services.Base;
using Hackathon.Application.Services.Staff;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAdminServices();
        services.AddBaseServices();
        services.AddStaffServices();
        return services;
    }
}
