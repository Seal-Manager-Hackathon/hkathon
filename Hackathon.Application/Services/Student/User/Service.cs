using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.User;

public class Service : IUserService
{
    private readonly IUserRepository _userRepository;

    public Service(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<StudentUserDetailResponse> GetUserDetail(Guid userId)
    {

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

    public async Task<SearchUsersResponse> SearchUsers(string? keyword, int pageIndex, int pageSize)
    {
        PaginationHelper.Validate(pageIndex, pageSize);

        var (items, totalCount) = await _userRepository.SearchByEmailAsync(
            keyword, false, pageIndex, pageSize);

        return new SearchUsersResponse
        {
            Users = items.Select(u => new StudentUserItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? null : u.AvatarUrl,
                College = string.IsNullOrEmpty(u.College) ? null : u.College,
                StudentId = string.IsNullOrEmpty(u.StudentId) ? null : u.StudentId,
                IsVerified = u.IsVerified ?? false,
                CreatedAt = u.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
