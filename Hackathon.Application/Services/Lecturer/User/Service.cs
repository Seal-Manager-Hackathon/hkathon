using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.User;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.User;

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
        _authorizationService.Authorize(RoleEnum.Lecturer);

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

    public async Task<GetUserCountResponse> GetUserCount(GetUserCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<RoleEnum>(request.Role, true, out var parsedRole))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsedRole;
        }

        var total = await _userRepository.CountByRoleAsync(role);

        return new GetUserCountResponse
        {
            Total = total
        };
    }
}