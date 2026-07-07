using Hackathon.Application.Services.Base.Auth;
using Hackathon.Application.Services.Base.User;
using Hackathon.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Base;

public static class DependencyInjection
{
    public static IServiceCollection AddBaseServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, Auth.Service>();
        services.AddScoped<IAuthorizationService, Auth.AuthorizationService>();
        services.AddScoped<IUserProfileService, User.Service>();
        return services;
    }
}
