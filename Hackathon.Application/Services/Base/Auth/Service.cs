using System.Security.Claims;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EmailVerification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;
using SuccMsg = Hackathon.Application.Common.SuccessMessage;

namespace Hackathon.Application.Services.Base.Auth;

public class Service : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IResetPasswordRepository _resetPasswordRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IMailService _mailService;
    private readonly ICurrentUserService _currentUserService;

    public Service(
        IUserRepository userRepository,
        IEmailVerificationRepository emailVerificationRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IResetPasswordRepository resetPasswordRepository,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtService jwtService,
        IMailService mailService,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _emailVerificationRepository = emailVerificationRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _resetPasswordRepository = resetPasswordRepository;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _mailService = mailService;
        _currentUserService = currentUserService;
    }

    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email.ToLower());

        if (existingUser != null)
        {
            if (existingUser.IsVerified == true)
            {
                throw new ConflictException(ErrMsg.Auth.EmailAlreadyExists);
            }
            throw new ConflictException(ErrMsg.Auth.UnverifiedAccountPleaseLoginToVerify);
        }

        var now = DateTimeOffset.UtcNow;
        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashPassword = _passwordService.HashPassword(request.Password),
            Role = RoleEnum.Student,
            IsVerified = false,
            Status = UserStatusEnum.Active,
            AvatarUrl = $"https://robohash.org/{request.Email}",
            College = "FPT University",
            CreatedAt = now,
            UpdatedAt = now
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await SendVerificationEmailAsync(user);

        return new RegisterResponse
        {
            Message = SuccMsg.Auth.RegisterSuccessful
        };
    }

    public async Task<VerifyEmailResponse> VerifyEmail(VerifyEmailRequest request)
    {
        var principal = _jwtService.ValidateToken(request.Token);
        if (principal == null)
        {
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);
        }

        var userIdStr = principal.FindFirst("UserId")?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
        {
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);
        }

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);
        }

        var emailVerification = await _emailVerificationRepository.GetByUserIdAsync(userId);
        if (emailVerification == null)
        {
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);
        }

        if (user.IsVerified == true)
        {
            return new VerifyEmailResponse();
        }

        var now = DateTimeOffset.UtcNow;
        user.IsVerified = true;
        user.VerifyEmailAt = now;
        user.UpdatedAt = now;
        await _userRepository.UpdateAsync(user);

        emailVerification.Status = EmailVerificationStatusEnum.Verified;
        emailVerification.UpdatedAt = now;
        await _emailVerificationRepository.UpdateRangeAsync([emailVerification]);

        await _unitOfWork.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString())
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        return new VerifyEmailResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email.Trim().ToLower());

        if (user == null)
        {
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);
        }

        if (user.Status == UserStatusEnum.Banned)
        {
            throw new ForbiddenException(ErrMsg.Auth.UserIsBanned);
        }

        var isPasswordValid = _passwordService.VerifyPassword(request.Password, user.HashPassword);
        if (!isPasswordValid)
        {
            throw new UnauthorizedException(ErrMsg.Auth.InvalidEmailOrPassword);
        }

        // If unverified, disable old pending verifications and send new one
        if (user.IsVerified == false)
        {
            var oldVerifications = await _emailVerificationRepository.GetPendingByUserIdAsync(user.Id);
            if (oldVerifications.Count > 0)
            {
                var now = DateTimeOffset.UtcNow;
                foreach (var old in oldVerifications)
                {
                    old.IsDisable = true;
                    old.UpdatedAt = now;
                }
                await _emailVerificationRepository.UpdateRangeAsync(oldVerifications);
            }

            await SendVerificationEmailAsync(user);
            throw new UnauthorizedException(ErrMsg.Auth.EmailUnverifiedOtpSent);
        }

        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString())
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshTokens
        {
            Id = Guid.NewGuid(),
            RefreshTokenHash = refreshToken,
            UserId = user.Id,
            DeviceLabel = string.Empty,
            RevokedAt = null,
            ExpiredAt = DateTimeOffset.UtcNow.AddDays(7),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<CurrentUserResponse> GetCurrentUser()
    {
        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);
        }

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user == null)
        {
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);
        }

        return new CurrentUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            AvatarUrl = user.AvatarUrl,
            Bio = user.Bio,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            StudentId = user.StudentId,
            College = user.College,
            ImgUrl = user.ImgUrl,
            LinkUrl = user.LinkUrl,
            Role = user.Role.ToString(),
            VerifyEmailAt = user.VerifyEmailAt,
            Status = user.Status?.ToString(),
            BanReason = user.BanReason,
            BannedAt = user.BannedAt,
            IsVerified = user.IsVerified
        };
    }

    private async Task SendVerificationEmailAsync(Users user)
    {
        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString()),
            new("IsVerified", user.IsVerified!.ToString()!.ToLower())
        };

        var emailToken = _jwtService.GenerateEmailVerificationToken(claims, 2);

        var emailVerification = new EmailVerifications
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = emailToken,
            ExpiredAt = DateTimeOffset.UtcNow.AddMinutes(5),
            Status = EmailVerificationStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _emailVerificationRepository.AddAsync(emailVerification);
        await _unitOfWork.SaveChangesAsync();

        await _mailService.SendVerificationEmailAsync(user.Email, emailToken);
    }

    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var user = await _userRepository.GetByEmailAsync(email);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new("UserId", user.Id.ToString())
            };
            var resetToken = _jwtService.GenerateEmailVerificationToken(claims, 2);
            var now = DateTimeOffset.UtcNow;

            var resetPassword = new ResetPasswords
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TokenHash = resetToken,
                IsUsed = false,
                ExpiresAt = now.AddMinutes(2),
                CreatedAt = now,
                UpdatedAt = now
            };

            await _resetPasswordRepository.AddAsync(resetPassword);
            await _unitOfWork.SaveChangesAsync();

            await _mailService.SendResetPasswordEmailAsync(user.Email, resetToken);
        }

        // Always return success to prevent email enumeration
    }

    public async Task ResetPassword(ResetPasswordRequest request)
    {
        var principal = _jwtService.ValidateToken(request.Token);
        if (principal == null)
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);

        var userIdStr = principal.FindFirst("UserId")?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);

        var resetPassword = await _resetPasswordRepository.GetByTokenAsync(request.Token);
        if (resetPassword == null)
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);

        if (resetPassword.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new BadRequestException(ErrMsg.Auth.InvalidOrExpiredEmailVerificationToken);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        var isSameAsOld = _passwordService.VerifyPassword(request.NewPassword, user.HashPassword);
        if (isSameAsOld)
            throw new BadRequestException(ErrMsg.Auth.NewPasswordMustBeDifferentFromOldPassword);

        var now = DateTimeOffset.UtcNow;
        user.HashPassword = _passwordService.HashPassword(request.NewPassword);
        user.UpdatedAt = now;

        resetPassword.IsUsed = true;
        resetPassword.UpdatedAt = now;

        await _userRepository.UpdateAsync(user);
        await _resetPasswordRepository.UpdateAsync(resetPassword);
        await _unitOfWork.SaveChangesAsync();
    }
}
