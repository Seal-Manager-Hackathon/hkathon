using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.User;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.User;

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

    public async Task<GetRecentUsersResponse> GetRecentUsers()
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var users = await _userRepository.GetRecentAsync(10);

        return new GetRecentUsersResponse
        {
            Users = users.Select(u => new RecentUserItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? null : u.AvatarUrl,
                Role = u.Role.ToString(),
                CreatedAt = u.CreatedAt
            }).ToList()
        };
    }

    public async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<RoleEnum>(request.Role, true, out var parsedRole))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsedRole;
        }

        var (items, totalCount) = await _userRepository.SearchAsync(
            request.Keyword, role, request.IsDisable, request.IsVerified, request.IsBanned,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetAllUsersResponse
        {
            Users = items.Select(u => new UserCard
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role.ToString(),
                Status = u.Status?.ToString(),
                IsVerified = u.IsVerified,
                IsDisable = u.IsDisable,
                BanReason = u.BanReason,
                BannedAt = u.BannedAt,
                AvatarUrl = u.AvatarUrl,
                College = u.College,
                CreatedAt = u.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<UserDetailResponse> GetUserDetail(GetUserDetailRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        return new UserDetailResponse
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