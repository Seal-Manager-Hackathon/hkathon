using Hackathon.Application.Services.Mentor.RegisterTeam;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application.Services.Mentor;

public static class DependencyInjection
{
    public static IServiceCollection AddMentorServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterTeamService, RegisterTeam.Service>();
        return services;
    }
}
