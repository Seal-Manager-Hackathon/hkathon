using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Assign;

public class Service : IAssignService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAssignEventRepository assignEventRepository,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _assignEventRepository = assignEventRepository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }

    public async Task<GetAvailableUserResponse> GetAvailableStaff(GetAvailableUserRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _userRepository.GetAvailableUsersByRoleAsync(
            request.EventId, RoleEnum.Staff, request.Keyword, request.PageIndex, request.PageSize);

        return MapResponse(items, totalCount, request);
    }

    public async Task<GetAvailableUserResponse> GetAvailableLecturer(GetAvailableUserRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var (items, totalCount) = await _userRepository.GetAvailableUsersByRoleAsync(
            request.EventId, RoleEnum.Lecturer, request.Keyword, request.PageIndex, request.PageSize);

        return MapResponse(items, totalCount, request);
    }

    public async Task AssignUserToEvent(AssignUserToEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        // Check user exists
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        // Check role hợp lệ
        if (user.Role == RoleEnum.Student)
            throw new BadRequestException("Cannot Assign Student To Event");
        if (user.Role == RoleEnum.Admin)
            throw new BadRequestException("Cannot Assign Admin To Event");

        // Check already assigned
        var existing = await _assignEventRepository.GetByEventIdAndUserIdAsync(request.EventId, request.UserId);
        if (existing != null)
            throw new ConflictException("User Is Already Assigned To This Event");

        Guid? eventRoleId = null;

        // Nếu là Staff → gán EventRole = Staff
        if (user.Role == RoleEnum.Staff)
        {
            var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(EventRoleEnum.Staff);
            if (eventRole == null)
                throw new NotFoundException("Event Role Staff Not Found");
            eventRoleId = eventRole.Id;
        }
        // Nếu là Lecturer → ko gán EventRole (EventRoleId = null)

        var assignEvent = new AssignEvents
        {
            Id = Guid.NewGuid(),
            EventId = request.EventId,
            UserId = request.UserId,
            EventRoleId = eventRoleId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _assignEventRepository.Add(assignEvent);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignEventRoleToLecturer(AssignEventRoleToLecturerRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        // Check assign event exists
        var assignEvent = await _assignEventRepository.GetByIdAsync(request.AssignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Check user có phải Lecturer ko
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("User Is Not A Lecturer");

        // Parse event role
        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid Event Role. Must be: Judge, Mentor");

        // Ko cho phép Staff
        if (eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Cannot Assign Staff Role To Lecturer");

        // Get EventRole từ DB
        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        // Update EventRoleId
        assignEvent.EventRoleId = eventRole.Id;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetAssignedUsersResponse> GetAssignedUsers(GetAssignedUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        Domain.Enums.User.RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<Domain.Enums.User.RoleEnum>(request.Role, true, out var parsedRole))
                throw new Exception("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsedRole;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersAsync(
            request.EventId, request.Keyword, role, request.PageIndex, request.PageSize);

        return new GetAssignedUsersResponse
        {
            Items = items.Select(ae => new AssignedUserItem
            {
                AssignEventId = ae.Id,
                UserId = ae.User.Id,
                Email = ae.User.Email,
                FirstName = ae.User.FirstName,
                LastName = ae.User.LastName,
                AvatarUrl = string.IsNullOrEmpty(ae.User.AvatarUrl) ? null : ae.User.AvatarUrl,
                EventRole = ae.EventRole?.Name.ToString(),
                AssignTracks = ae.AssignTracks.Select(at => new AssignedTrackItem
                {
                    TrackId = at.TrackId,
                    Title = at.Track.Title
                }).ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    private static GetAvailableUserResponse MapResponse(List<Domain.Entities.Users> items, int totalCount, GetAvailableUserRequest request)
    {
        return new GetAvailableUserResponse
        {
            Items = items.Select(u => new AvailableUserItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? null : u.AvatarUrl,
                College = string.IsNullOrEmpty(u.College) ? null : u.College,
                PhoneNumber = string.IsNullOrEmpty(u.PhoneNumber) ? null : u.PhoneNumber
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
