using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.User;

public class Service : IUserProfileService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUserService _currentUserService;

    public Service(
        IUserRepository userRepository,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
    }

    public async Task<GetMyProfileResponse> GetMyProfile()
    {
        _authorizationService.Authenticate();

        var userId = _currentUserService.UserId;
        if (userId == null)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        return new GetMyProfileResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? null : user.AvatarUrl,
            Bio = user.Bio,
            Role = user.Role.ToString(),
            Status = user.Status?.ToString(),
            IsVerified = user.IsVerified,
            College = string.IsNullOrEmpty(user.College) ? null : user.College,
            CreatedAt = user.CreatedAt
        };
    }
}
