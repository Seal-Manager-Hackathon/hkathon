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
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IUserRepository userRepository,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
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

    public async Task UpdateProfile(UpdateProfileRequest request)
    {
        _authorizationService.Authenticate();

        var userId = _currentUserService.UserId;
        if (userId == null)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var user = await _userRepository.GetByIdAsync(userId.Value);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (request.FirstName != null)
            user.FirstName = request.FirstName;
        if (request.LastName != null)
            user.LastName = request.LastName;
        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;
        if (request.Bio != null)
            user.Bio = request.Bio;
        if (request.Address != null)
            user.Address = request.Address;
        if (request.DateOfBirth.HasValue)
            user.DateOfBirth = request.DateOfBirth.Value;
        if (request.StudentId != null)
        {
            if (!string.IsNullOrEmpty(user.StudentId))
                throw new BadRequestException("Student Id Cannot Be Changed Once Set");
            user.StudentId = request.StudentId;
        }
        if (request.ImgUrl != null)
            user.ImgUrl = request.ImgUrl;
        if (request.LinkUrl != null)
            user.LinkUrl = request.LinkUrl;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }
}
