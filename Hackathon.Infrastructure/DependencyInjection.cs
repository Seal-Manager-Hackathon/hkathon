using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Infrastructure.Services.Cloudinary;
using Hackathon.Infrastructure.Services.Jwt;
using Hackathon.Infrastructure.Services.Mail;
using Hackathon.Infrastructure.Services.Password;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        services.AddScoped<IJwtService, Services.Jwt.Service>();

        services.Configure<MailOptions>(configuration.GetSection("MailOptions"));
        services.AddScoped<IMailService, Services.Mail.Service>();

        services.Configure<CloudinaryOptions>(configuration.GetSection("CloudinaryOptions"));
        services.AddScoped<IMediaService, Services.Cloudinary.Service>();

        services.Configure<SecurityOptions>(configuration.GetSection("SecurityOptions"));
        services.AddScoped<IPasswordService, Services.Password.Service>();

        services.AddScoped<IUserRepository, Repositories.UserRepository>();
        services.AddScoped<IEmailVerificationRepository, Repositories.EmailVerificationRepository>();
        services.AddScoped<IRefreshTokenRepository, Repositories.RefreshTokenRepository>();
        services.AddScoped<IEventRepository, Repositories.EventRepository>();
        services.AddScoped<ITeamRepository, Repositories.TeamRepository>();
        services.AddScoped<IReportRepository, Repositories.ReportRepository>();
        services.AddScoped<INotificationRepository, Repositories.NotificationRepository>();
        services.AddScoped<IRegisterTeamRepository, Repositories.RegisterTeamRepository>();
        services.AddScoped<IRoundRepository, Repositories.RoundRepository>();
        services.AddScoped<IResetPasswordRepository, Repositories.ResetPasswordRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, Services.CurrentUserService>();

        return services;
    }
}
