using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.User;

public class Service : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _authorizationService = authorizationService;
    }

    public async Task<StudentUserDetailResponse> GetUserDetail(Guid userId)
    {
        _authorizationService.Authorize(RoleEnum.Student);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        // Student: ko cho xem user bi disable, nhung user bi ban van visible
        if (user.IsDisable)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        return new StudentUserDetailResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? null : user.PhoneNumber,
            AvatarUrl = string.IsNullOrEmpty(user.AvatarUrl) ? null : user.AvatarUrl,
            Bio = user.Bio,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth == DateTimeOffset.MinValue ? null : user.DateOfBirth,
            StudentId = string.IsNullOrEmpty(user.StudentId) ? null : user.StudentId,
            College = string.IsNullOrEmpty(user.College) ? null : user.College,
            ImgUrl = user.ImgUrl,
            LinkUrl = user.LinkUrl,
            Role = user.Role.ToString(),
            Status = user.Status?.ToString(),
            IsVerified = user.IsVerified,
            IsDisable = user.IsDisable,
            BanReason = user.BanReason,
            BannedAt = user.BannedAt,
            VerifyEmailAt = user.VerifyEmailAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
