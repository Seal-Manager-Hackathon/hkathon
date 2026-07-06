using Hackathon.Application.Common.Interfaces;
using Hackathon.Infrastructure.Services.Jwt;
using Hackathon.Infrastructure.Services.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtOption>(configuration.GetSection("JwtOptions"));
        services.AddScoped<IJwtService, Services.Jwt.Service>();

        services.Configure<MailOption>(configuration.GetSection("MailOptions"));
        services.AddScoped<IMailService, Services.Mail.Service>();

        return services;
    }
}
