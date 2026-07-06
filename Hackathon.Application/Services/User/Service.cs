using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.User;

public class Service : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRecentUsersResponse> GetRecentUsers()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var users = await _userRepository.GetRecentAsync(5);

        return new GetRecentUsersResponse
        {
            Users = users.Select(u => new RecentUserItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role.ToString(),
                CreatedAt = u.CreatedAt
            }).ToList()
        };
    }

    public async Task<GetUserCountResponse> GetUserCount(GetUserCountRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        RoleEnum? role = null;

        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<RoleEnum>(request.Role, true, out var parsedRole))
            {
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            }
            role = parsedRole;
        }

        var total = await _userRepository.CountByRoleAsync(role);

        return new GetUserCountResponse
        {
            Total = total
        };
    }

    public async Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<RoleEnum>(request.Role, true, out var parsedRole))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsedRole;
        }

        var (items, totalCount) = await _userRepository.SearchAsync(
            request.Keyword, role, request.IsDisable, request.IsVerified,
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
                AvatarUrl = u.AvatarUrl,
                College = u.College,
                CreatedAt = u.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var existingUser = await _userRepository.GetByEmailAsync(request.Email.ToLower());
        if (existingUser != null)
        {
            throw new ConflictException(ErrMsg.Auth.EmailAlreadyExists);
        }

        if (!Enum.TryParse<RoleEnum>(request.Role, true, out var role))
        {
            throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
        }

        var now = DateTimeOffset.UtcNow;
        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashPassword = _passwordService.HashPassword(request.Password),
            Role = role,
            IsVerified = true,
            Status = UserStatusEnum.Active,
            AvatarUrl = $"https://robohash.org/{request.Email}",
            College = request.College ?? "FPT University",
            CreatedAt = now,
            UpdatedAt = now
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new CreateUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            College = user.College
        };
    }
}
