using System.Security.Claims;
using System.Text;
using Hackathon.Domain.Enums.User;
using Hackathon.Infrastructure.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.Presentation.Extentions;

public static class JwtExtensions
{
    public const string AdminPolicy = "AdminPolicy";
    public const string StaffPolicy = "StaffPolicy";
    public const string LecturerPolicy = "LecturerPolicy";
    public const string StudentPolicy = "StudentPolicy";
    public const string StaffOrAdminPolicy = "StaffOrAdminPolicy";
    public const string StudentVerifiedPolicy = "StudentVerifiedPolicy";
    public const string StaffLecturerOrAdminPolicy = "StaffLecturerOrAdminPolicy";

    public static void AddJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        JwtOptions jwtOption = new JwtOptions();
        configuration.GetSection("JwtOptions").Bind(jwtOption);
        var key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,//dung signature, dung issure nua, dung server
                    ValidateAudience = true, // 
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {

                        if (context.Request.Cookies.TryGetValue(CookieExtensions.AccessTokenCookieName, out var token))
                        {
                            context.Token = token;
                        }


                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AdminPolicy, policy =>
                policy.RequireRole(RoleEnum.Admin.ToString()));
            // [Authorize(Policy = JwtExtensions.AdminPolicy)]

            options.AddPolicy(StaffPolicy, policy =>
                policy.RequireRole(RoleEnum.Staff.ToString()));
            // [Authorize(Policy = JwtExtensions.StaffPolicy)]

            options.AddPolicy(StudentPolicy, policy =>
                policy.RequireRole(RoleEnum.Student.ToString()));

            options.AddPolicy(LecturerPolicy, policy =>
                policy.RequireRole(RoleEnum.Lecturer.ToString()));

            options.AddPolicy(StaffOrAdminPolicy, policy =>
                policy.RequireRole(RoleEnum.Staff.ToString(), RoleEnum.Admin.ToString()));
            // [Authorize(Policy = JwtExtensions.StaffOrAdminPolicy)]

            options.AddPolicy(StaffLecturerOrAdminPolicy, policy =>
                policy.RequireRole(RoleEnum.Staff.ToString(),
                                   RoleEnum.Lecturer.ToString(),
                                   RoleEnum.Admin.ToString()));
            // [Authorize(Policy = JwtExtensions.StaffLecturerOrAdminPolicy)]

            options.AddPolicy(StudentVerifiedPolicy, policy =>
                policy.RequireRole(RoleEnum.Student.ToString())
                    .RequireClaim("IsVerified", "true"));
        });
    }
}