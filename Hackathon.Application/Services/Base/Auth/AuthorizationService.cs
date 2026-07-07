using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Base.Auth;

public class AuthorizationService : IAuthorizationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public AuthorizationService(ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public void Authenticate()
    {
        if (!_currentUserService.IsAuthenticated)
        {
            throw new UnauthorizedException(ErrorMessage.Auth.InvalidOrExpiredToken);
        }
    }

    public void Authorize(params RoleEnum[] allowedRoles)
    {
        Authenticate();

        var userId = _currentUserService.UserId;
        if (userId == null)
        {
            throw new UnauthorizedException(ErrorMessage.Auth.InvalidOrExpiredToken);
        }

        var user = _userRepository.GetByIdAsync(userId.Value).GetAwaiter().GetResult();
        if (user == null)
        {
            throw new NotFoundException(ErrorMessage.Auth.UserNotFound);
        }

        if (!allowedRoles.Contains(user.Role))
        {
            throw new ForbiddenException("You do not have permission to perform this action");
        }
    }
}
