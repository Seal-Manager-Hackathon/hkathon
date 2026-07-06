using System.Security.Claims;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EmailVerification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Auth;

public class Service : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationRepository _emailVerificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IMailService _mailService;

    public Service(
        IUserRepository userRepository,
        IEmailVerificationRepository emailVerificationRepository,
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        IJwtService jwtService,
        IMailService mailService)
    {
        _userRepository = userRepository;
        _emailVerificationRepository = emailVerificationRepository;
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _mailService = mailService;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
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
            Message = ErrMsg.Auth.RegistrationSuccessful
        };
    }

    public async Task<VerifyEmailResponse> VerifyEmailAsync(VerifyEmailRequest request)
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
            new("UserId", user.Id.ToString()),
            // new(ClaimTypes.Role, user.Role.ToString()),
            new("IsVerified", user.IsVerified.ToString()!.ToLower())
        };

        var accessToken = _jwtService.GenerateAccessToken(claims);
        var refreshToken = _jwtService.GenerateRefreshToken();

        return new VerifyEmailResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task SendVerificationEmailAsync(Users user)
    {
        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString()),
            // new(ClaimTypes.Role, user.Role.ToString()),
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
}
