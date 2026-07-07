using Hackathon.Application.Common.Helpers;
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
    private readonly ICurrentUserService _currentUserService;
    private readonly IMediaService _mediaService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IUserRepository userRepository,
        IPasswordService passwordService,
        IAuthorizationService authorizationService,
        ICurrentUserService currentUserService,
        IMediaService mediaService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _authorizationService = authorizationService;
        _currentUserService = currentUserService;
        _mediaService = mediaService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRecentUsersResponse> GetRecentUsers()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<RoleEnum>(request.Role, true, out var parsedRole))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsedRole;
        }

        var (items, totalCount) = await _userRepository.SearchAsync(
            request.Keyword, role, request.IsDisable, request.IsVerified,
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
                AvatarUrl = u.AvatarUrl,
                College = u.College,
                CreatedAt = u.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
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

    public async Task<UserDetailResponse> GetUserDetail(GetUserDetailRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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

    public async Task UpdateUser(UpdateUserRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        // Chỉ update các field được phép
        if (request.FirstName != null)
            user.FirstName = request.FirstName;
        if (request.LastName != null)
            user.LastName = request.LastName;
        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;
        if (request.AvatarFile != null)
            user.AvatarUrl = await _mediaService.UploadImageAsync(request.AvatarFile, "avatars");
        if (request.Bio != null)
            user.Bio = request.Bio;
        if (request.Address != null)
            user.Address = request.Address;
        if (request.DateOfBirth.HasValue)
            user.DateOfBirth = request.DateOfBirth.Value;
        if (request.StudentId != null)
            user.StudentId = request.StudentId;
        if (request.College != null)
            user.College = request.College;
        if (request.ImgUrl != null)
            user.ImgUrl = request.ImgUrl;
        if (request.LinkUrl != null)
            user.LinkUrl = request.LinkUrl;
        if (request.Status != null)
        {
            if (!Enum.TryParse<UserStatusEnum>(request.Status, true, out var status))
                throw new BadRequestException("Invalid Status. Must be: Active, Inactive");
            user.Status = status;
        }
        if (request.IsDisable.HasValue)
            user.IsDisable = request.IsDisable.Value;

        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        user.IsDisable = true;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreUser(Guid userId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        user.IsDisable = false;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task BanUser(BanUserRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        user.BanReason = request.BanReason;
        user.BannedAt = DateTimeOffset.UtcNow;
        // IsDisable vẫn false — ban ko ẩn user
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UnbanUser(Guid userId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        user.BanReason = null;
        user.BannedAt = null;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
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
            HashPassword = _passwordService.HashPassword("string"),
            Role = role,
            IsVerified = true,
            Status = UserStatusEnum.Active,
            AvatarUrl = $"https://robohash.org/{request.Email}",
            College = "FPT University",
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
            Password = "string",
            Role = user.Role.ToString(),
            College = user.College
        };
    }
}
