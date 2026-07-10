using Hackathon.Application.Common.Helpers.Leaderboard;
using Hackathon.Application.Services.Admin;
using Hackathon.Application.Services.Base;
using Hackathon.Application.Services.Lecturer;
using Hackathon.Application.Services.Mentor;
using Hackathon.Application.Services.Staff;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<LeaderboardHelper>();
        services.AddAdminServices();
        services.AddBaseServices();
        services.AddStaffServices();
        services.AddLecturerServices();
        services.AddMentorServices();
        return services;
    }
}
