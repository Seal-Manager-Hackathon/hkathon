using System.Security.Claims;
using Hackathon.Repository;
using Hackathon.Repository.Entity;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.MailServices;
using Hackathon.Service.Models;
using Hackathon.Service.Auths;
using Hackathon.Service.JwtServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using IService = Hackathon.Service.Auths.IService;

namespace Hackathon.Service.Auths;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly SecurityOption _securityOptions = new();
    private readonly JwtServices.IService _jwtService;
    private readonly MailServices.IService _mailService;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext, IConfiguration configuration,
        MailServices.IService mailService, IHttpContextAccessor httpContextAccessor,
        JwtServices.IService jwtService)
    {
        _dbContext = dbContext;
        configuration.GetSection(nameof(SecurityOption)).Bind(_securityOptions);
        _jwtService = jwtService;
        _mailService = mailService;
        _httpContext = httpContextAccessor;
    }
    
    private bool CheckExpiredAccessToken()
    {
        var check = _httpContext.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Expiration);
        if (check != null)
        {
            if (long.TryParse(check.Value, out long expUnixTime))
            {
                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime);
                if (expirationTime > DateTimeOffset.UtcNow)
                {
                    // Token is STILL VALID
                    Console.WriteLine("Token is still valid.");
                    return false;
                }
                return true;
            }
        }
        return true;
    }

    private async Task SendVerificationEmailAsync(Repository.Entity.Users user, string email)
    {
        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("IsVerified", user.IsVerified.ToString().ToLower()),
        };
        var emailToken = _jwtService.GenerateEmailVerificationToken(claims, 2);
        
        var newEmailVerification = new Repository.Entity.EmailVerifications()
        {
            UserId = user.Id,
            TokenHash = emailToken,
            ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(2),
            Status = EmailVerificationStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        
        await _dbContext.EmailVerifications.AddAsync(newEmailVerification);
        await _dbContext.SaveChangesAsync();
        
        await _mailService.SendMail(new MailContent
        {
            To = email,
            Subject = "Account Verification - SEAL Hackathon",
            Body = MailTemplate.EmailContainToken(emailToken),
        });
    }

    private string CheckRefreshToken()
    {
        _httpContext.HttpContext!.Request.Cookies.TryGetValue("Refresh-Token", out var check);
        if (check == null)
        {
            throw new BadRequestException("REFRESH_TOKEN_MISSING");
        }

        return check;
    }

    public async Task<string> Register(Request.RegisterRequest request)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower() == request.Email.ToLower());

            if (existingUser != null)
            {
                if (existingUser.IsVerified == true)
                {
                    throw new ConflictException("EMAIL_ALREADY_EXISTS");
                }

                throw new ConflictException("UNVERIFIED_ACCOUNT_PLEASE_LOGIN_TO_VERIFY");
            }

            var pepperPassword = request.Password + _securityOptions.Pepper;
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(pepperPassword, hashType: BCrypt.Net.HashType.SHA256);

            var newUser = new Repository.Entity.Users()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                HashPassword = hashedPassword,
                Role = RoleEnum.Student,
                IsVerified = false,
                Status = UserStatusEnum.Active,
                AvatarUrl = $"https://robohash.org/{request.Email}",
                College = "FPT University",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            await SendVerificationEmailAsync(newUser, request.Email);

            await transaction.CommitAsync();
            return "REGISTRATION_SUCCESSFUL";
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    public async Task<Response.AuthResponse> RefreshToken()
    {
        // Return false means: HAS ACCESS AND STILL VALID
        bool isMissingAccessToken = CheckExpiredAccessToken();

        if (!isMissingAccessToken)
        {// If returns true: Means NO ACCESS TOKEN -> Logic automatically proceeds to step 2
            throw new BadRequestException("ACCESS_TOKEN_STILL_VALID");
        }

        var rawRefreshToken = CheckRefreshToken();

        var storedToken = await _dbContext.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.RefreshTokenHash == rawRefreshToken);

        if (storedToken == null)
        {
            throw new ExpiredRefreshTokenException();
        }

        bool isActive = storedToken.RevokedAt == null && storedToken.ExpiredAt > DateTimeOffset.UtcNow;

        if (!isActive)
        {
            throw new ExpiredRefreshTokenException();
        }

        storedToken.RevokedAt = DateTimeOffset.UtcNow;
        await _dbContext.SaveChangesAsync();

        string newRawRefreshToken = _jwtService.GenerateRefreshToken();

        var newRefreshTokenEntity = new Hackathon.Repository.Entity.RefreshTokens()
        {
            RefreshTokenHash = newRawRefreshToken, // Store raw string directly based on system configuration
            UserId = storedToken.User.Id,
            IpAddress = _httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
            UserAgent = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown",
            DeviceLabel = storedToken.DeviceLabel,
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(15),
            CreatedAt = DateTimeOffset.UtcNow,
        };
        _dbContext.RefreshTokens.Add(newRefreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        var claimsForNewToken = new List<Claim>
        {
            new Claim("UserId", storedToken.User.Id.ToString()),
            new Claim(ClaimTypes.Role, storedToken.User.Role.ToString()),
            new Claim("IsVerified", storedToken.User.IsVerified.ToString().ToLower()),
        };
        string newAccessToken = _jwtService.GenerateAccessToken(claimsForNewToken);

        var result = new Response.AuthResponse()
        {
            RefreshToken = newRawRefreshToken,
            AccessToken = newAccessToken,
        };

        return result;
    }

    public async Task<Response.VerifyEmailResponse?> VerifyEmail(Request.VerifyEmailRequest request)
    {
        var validateToken = _jwtService.ValidateToken(request.Token);
        if (validateToken == null)
        {
            throw new BadRequestException("INVALID_OR_EXPIRED_EMAIL_VERIFICATION_TOKEN");
        }

        var userIdStr = validateToken.FindFirst("UserId")?.Value;
        var userId = Guid.Parse(userIdStr!);
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var emailValid = await _dbContext.EmailVerifications.FirstOrDefaultAsync(x => x.UserId == userId);
        if (user == null)
        {
            throw new NotFoundException("USER_NOT_FOUND");        
        }
    
        if (emailValid == null)
        {
            throw new NotFoundException("EMAILVALID_NOT_FOUND");        
        }
        if (user.IsVerified == true)
        {
            return new Response.VerifyEmailResponse
            {
                AccessToken = null!,
                RefreshToken = null!,
            };
        }

        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var now = DateTimeOffset.UtcNow;
            user.IsVerified = true;
            user.VerifyEmailAt = now;
            user.UpdatedAt = now;
            _dbContext.Users.Update(user);

            emailValid.Status = EmailVerificationStatusEnum.Verified;
            emailValid.UpdatedAt = now;
            _dbContext.EmailVerifications.Update(emailValid);

            var authClaims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("IsVerified", user.IsVerified.ToString().ToLower()),
            };

            var accessToken = _jwtService.GenerateAccessToken(authClaims);
            var refreshToken = _jwtService.GenerateRefreshToken();

            var httpContext = _httpContext.HttpContext;
            var ipAddress = httpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown IP";
            var userAgent = httpContext?.Request?.Headers["User-Agent"].ToString() ?? "Unknown Device";

            var refreshTokenEntity = new Repository.Entity.RefreshTokens()
            {
                Id = Guid.NewGuid(),
                RefreshTokenHash = refreshToken,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                RevokedAt = null,
                DeviceLabel = "",
                UserId = user.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                ExpiredAt = DateTimeOffset.UtcNow.AddDays(15),
            };
            await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);


            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return new Response.VerifyEmailResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private Guid CheckAccessToken()
    {
        var accessToken = _httpContext.HttpContext?.User.FindFirst("UserId")?.Value;
        if (accessToken != null)
        {
            return Guid.Parse(accessToken);
        }

        return Guid.Empty;
    }

    public async Task<Response.GetMeResponse> GetMe()
    {
        var userId = CheckAccessToken();
        if (userId == Guid.Empty)
        {
            throw new MissingAccessTokenException();
        }

        var query = await _dbContext.Users
            .Where(x => x.Id == userId)
            .Select(y => new Response.GetMeResponse()
            {
                Id = y.Id,
                Role = y.Role,
                Email = y.Email,
                FirstName = y.FirstName,
                LastName = y.LastName,
                Avatar = y.AvatarUrl,
            }).FirstOrDefaultAsync();

        if (query == null)
        {
            throw new NotFoundException("USER_NOT_FOUND");
        }

        return query;
    }

    public async Task<string> Logout()
    {
        var rtInCookie = CheckRefreshToken();

        var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x =>
            x.RefreshTokenHash == rtInCookie  && !x.IsDisable && x.RevokedAt == null);

        if (refreshToken == null)
        {
            throw new UnauthorizedException("INVALID_REFRESH_TOKEN");
        }

        if (refreshToken.RevokedAt != null)
        {
            throw new UnauthorizedException("USER_ALREADY_LOGGED_OUT");
        }

        refreshToken.RevokedAt = DateTimeOffset.UtcNow;
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync();

        return "LOGOUT_SUCCESSFUL";
    }
    
    public async Task<Response.LoginResponse> LoginAsync(Request.LoginRequest request)
    {
        var httpContext = _httpContext.HttpContext;
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        var userAgent = httpContext?.Request.Headers.UserAgent.ToString() ?? "Unknown Device";
        var email = request.Email.Trim();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDisable);

        if (user == null)
        {
            throw new NotFoundException("EMAIL_NOT_FOUND");
        }

        if (user.Status == Repository.Enum.UserStatusEnum.Banned)
        {
            throw new ForbiddenException("USER_IS_BANNED");
        }

        var pepperPassword = request.Password + _securityOptions.Pepper;
        var isPasswordValid = global::BCrypt.Net.BCrypt.EnhancedVerify(
            pepperPassword,
            user.HashPassword,
            hashType: global::BCrypt.Net.HashType.SHA256
        );

        if (!isPasswordValid)
        {
            throw new UnauthorizedException("INVALID_EMAIL_OR_PASSWORD");
        }

        if (user.IsVerified == false)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var oldVerifications = await _dbContext.EmailVerifications
                    .Where(x => x.UserId == user.Id && x.Status == EmailVerificationStatusEnum.Pending && !x.IsDisable)
                    .ToListAsync();

                if (oldVerifications.Count > 0)
                {
                    var now = DateTimeOffset.UtcNow;
                    foreach (var old in oldVerifications)
                    {
                        old.IsDisable = true;
                        old.UpdatedAt = now;
                    }
                    _dbContext.EmailVerifications.UpdateRange(oldVerifications);
                }

                await SendVerificationEmailAsync(user, email);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            throw new UnauthorizedException("EMAIL_UNVERIFIED_OTP_SENT");
        }

        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("IsVerified", user.IsVerified.ToString().ToLower()),
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new Repository.Entity.RefreshTokens
        {
            Id = Guid.NewGuid(),
            RefreshTokenHash = refreshToken,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceLabel = "",
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(15),
            UserId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        await _dbContext.RefreshTokens.AddAsync(refreshTokenEntity);
        await _dbContext.SaveChangesAsync();

        var result = new Response.LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        return result;
    }
    public async Task<string> ChangePassword(Request.ChangePasswordRequest request)
    {
        var userId = CheckAccessToken();
        if (userId == Guid.Empty)
        {
            throw new MissingAccessTokenException();
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDisable);
        if (user == null)
        {
            throw new NotFoundException("USER_NOT_FOUND");
        }

        var currentPepperPassword = request.CurrentPassword + _securityOptions.Pepper;
        var isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(
            currentPepperPassword,
            user.HashPassword,
            hashType: BCrypt.Net.HashType.SHA256
        );

        if (!isPasswordValid)
        {
            throw new BadRequestException("CURRENT_PASSWORD_INVALID");
        }

        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var newPepperPassword = request.NewPassword + _securityOptions.Pepper;
            user.HashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(newPepperPassword, hashType: BCrypt.Net.HashType.SHA256);
            user.UpdatedAt = DateTimeOffset.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return "PASSWORD_CHANGED_SUCCESSFULLY";
    }

    public async Task<string> ForgotPassword(Request.ForgotPasswordRequest request)
    {
        var email = request.Email.Trim();

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDisable);
        if (user != null)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                };
                var resetToken = _jwtService.GenerateEmailVerificationToken(claims, 2);
                var now = DateTimeOffset.UtcNow;

                var resetPassword = new Repository.Entity.ResetPasswords
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    TokenHash = resetToken,
                    IsUsed = false,
                    ExpiresAt = now.AddMinutes(2),
                    CreatedAt = now,
                    UpdatedAt = now
                };

                await _dbContext.ResetPasswords.AddAsync(resetPassword);
                await _dbContext.SaveChangesAsync();

                await _mailService.SendMail(new MailContent
                {
                    To = email,
                    Subject = "Reset Password - SEAL Hackathon",
                    Body = MailTemplate.ForgotPasswordContainToken(resetToken),
                });

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return "FORGOT_PASSWORD_REQUEST_ACCEPTED";
    }

    public async Task<string> ResetPassword(Request.ResetPasswordRequest request)
    {
        var validateToken = _jwtService.ValidateToken(request.Token);
        if (validateToken == null)
        {
            throw new BadRequestException("INVALID_OR_EXPIRED_RESET_PASSWORD_TOKEN");
        }

        var userIdStr = validateToken.FindFirst("UserId")?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            throw new BadRequestException("INVALID_RESET_PASSWORD_TOKEN");
        }

        var resetPassword = await _dbContext.ResetPasswords.FirstOrDefaultAsync(x =>
            x.UserId == userId &&
            x.TokenHash == request.Token &&
            !x.IsUsed &&
            !x.IsDisable);
        if (resetPassword == null)
        {
            throw new BadRequestException("INVALID_OR_USED_RESET_PASSWORD_TOKEN");
        }

        if (resetPassword.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            throw new BadRequestException("EXPIRED_RESET_PASSWORD_TOKEN");
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId && !x.IsDisable);
        if (user == null)
        {
            throw new NotFoundException("USER_NOT_FOUND");
        }

        var newPasswordWithPepper = request.NewPassword + _securityOptions.Pepper;
        var isSameAsOldPassword = BCrypt.Net.BCrypt.EnhancedVerify(
            newPasswordWithPepper,
            user.HashPassword,
            hashType: BCrypt.Net.HashType.SHA256
        );
        if (isSameAsOldPassword)
        {
            throw new BadRequestException("NEW_PASSWORD_MUST_BE_DIFFERENT_FROM_OLD_PASSWORD");
        }

        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var now = DateTimeOffset.UtcNow;
            user.HashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(newPasswordWithPepper, hashType: BCrypt.Net.HashType.SHA256);
            user.UpdatedAt = now;

            resetPassword.IsUsed = true;
            resetPassword.UpdatedAt = now;

            _dbContext.Users.Update(user);
            _dbContext.ResetPasswords.Update(resetPassword);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return "PASSWORD_RESET_SUCCESSFULLY";
    }

    public async Task<string> ResendEmailVerification(Request.ResendEmailVerificationRequest request)
    {
        var email = request.Email.Trim();

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower() && !x.IsDisable);
        if (user == null)
        {
            throw new NotFoundException("USER_NOT_FOUND");
        }

        if (user.IsVerified == true)
        {
            throw new BadRequestException("USER_ALREADY_VERIFIED");
        }

        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Vô hiệu hoá các verification token cũ đang pending
            var oldVerifications = await _dbContext.EmailVerifications
                .Where(x => x.UserId == user.Id && x.Status == EmailVerificationStatusEnum.Pending && !x.IsDisable)
                .ToListAsync();

            if (oldVerifications.Count > 0)
            {
                var nowTime = DateTimeOffset.UtcNow;
                foreach (var old in oldVerifications)
                {
                    old.IsDisable = true;
                    old.UpdatedAt = nowTime;
                }
                _dbContext.EmailVerifications.UpdateRange(oldVerifications);
            }

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("IsVerified", (user.IsVerified ?? false).ToString().ToLower()),
            };
            var emailToken = _jwtService.GenerateEmailVerificationToken(claims, 2);
            var now = DateTimeOffset.UtcNow;

            var emailVerification = new Repository.Entity.EmailVerifications
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = emailToken,
                ExpiredAt = now.AddMinutes(2),
                Status = EmailVerificationStatusEnum.Pending,
                IsDisable = false,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _dbContext.EmailVerifications.AddAsync(emailVerification);
            await _dbContext.SaveChangesAsync();

            await _mailService.SendMail(new MailContent
            {
                To = email,
                Subject = "Account Verification - SEAL Hackathon",
                Body = MailTemplate.EmailContainToken(emailToken),
            });

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return "EMAIL_VERIFICATION_SENT";
    }
}
