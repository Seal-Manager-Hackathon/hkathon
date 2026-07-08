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
    private readonly ITrackRepository _trackRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAssignEventRepository assignEventRepository,
        ITrackRepository trackRepository,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _assignEventRepository = assignEventRepository;
        _trackRepository = trackRepository;
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

        // Parse event role
        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid EventRole. Must be: Staff, Judge, Mentor");

        // Validate event role theo user role
        if (user.Role == RoleEnum.Staff && eventRoleEnum != EventRoleEnum.Staff)
            throw new BadRequestException("Staff Can Only Be Assigned Staff Role");

        if (user.Role == RoleEnum.Lecturer && eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Lecturer Cannot Be Assigned Staff Role");

        // Check already assigned
        var existing = await _assignEventRepository.GetByEventIdAndUserIdAsync(request.EventId, request.UserId);
        if (existing != null)
            throw new ConflictException("User Is Already Assigned To This Event");

        // Get EventRole từ DB
        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        var assignEvent = new AssignEvents
        {
            Id = Guid.NewGuid(),
            EventId = request.EventId,
            UserId = request.UserId,
            EventRoleId = eventRole.Id,
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

        Domain.Enums.EventRole.EventRoleEnum? eventRole = null;
        if (!string.IsNullOrWhiteSpace(request.EventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(request.EventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Mentor, Judge, Staff");
            eventRole = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersByEventAsync(
            request.EventId, request.Keyword, eventRole, request.PageIndex, request.PageSize);

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

    public async Task AssignTrackToEvent(AssignTrackToEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        // Check assign event exists
        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(request.AssignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff cannot be assigned to tracks
        if (assignEvent.EventRole?.Name == Domain.Enums.EventRole.EventRoleEnum.Staff)
            throw new BadRequestException("Staff Cannot Be Assigned To Track");

        // Check track exists
        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        // Check track cùng event với assign event
        if (track.EventId != assignEvent.EventId)
            throw new BadRequestException("Track Does Not Belong To The Same Event");

        // Check ko bị duplicate assign
        var isAssigned = await _assignEventRepository.IsTrackAssignedAsync(request.AssignEventId, request.TrackId);
        if (isAssigned)
            throw new ConflictException("Track Is Already Assigned To This User");

        var assignTrack = new AssignTracks
        {
            Id = Guid.NewGuid(),
            AssignEventId = request.AssignEventId,
            TrackId = request.TrackId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _assignEventRepository.AddAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTrackFromEvent(Guid assignEventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        // Check assign track exists
        var assignTrack = await _assignEventRepository.GetAssignTrackAsync(assignEventId, trackId);
        if (assignTrack == null)
            throw new NotFoundException("Assign Track Not Found");

        assignTrack.IsDisable = true;
        assignTrack.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.RemoveAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();
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
