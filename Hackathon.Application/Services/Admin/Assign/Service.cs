using Hackathon.Application.Common;
using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Notification;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Assign;

public class Service : IAssignService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAssignEventRepository assignEventRepository,
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _assignEventRepository = assignEventRepository;
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _notificationRepository = notificationRepository;
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

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (user.IsDisable)
            throw new BadRequestException("Cannot Assign A Disabled User");
        if (!string.IsNullOrEmpty(user.BanReason))
            throw new BadRequestException("Cannot Assign A Banned User");

        if (user.Role == RoleEnum.Student)
            throw new BadRequestException("Cannot Assign Student To Event");
        if (user.Role == RoleEnum.Admin)
            throw new BadRequestException("Cannot Assign Admin To Event");

        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid EventRole. Must be: Staff, Judge, Mentor");

        if (user.Role == RoleEnum.Staff && eventRoleEnum != EventRoleEnum.Staff)
            throw new BadRequestException("Staff Can Only Be Assigned Staff Role");

        if (user.Role == RoleEnum.Lecturer && eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Lecturer Cannot Be Assigned Staff Role");

        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        var existing = await _assignEventRepository.GetByEventIdAndUserIdAsync(request.EventId, request.UserId);

        if (existing != null)
        {
            if (existing.IsDisable)
            {
                // Re-enable + update role
                existing.IsDisable = false;
                existing.EventRoleId = eventRole.Id;
                existing.UpdatedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.SaveChangesAsync();

                // Gửi notification cho user được tái kích hoạt — ghi rõ event
                var existingEventName = existing.Event?.Name ?? "Event";
                var reenableNotif = NotificationHelper.Create(
                    NotificationTargetTypeEnum.Personal,
                    "Event Assignment Reactivated",
                    string.Format(NotificationMessage.Assignment.ReEnabled, existingEventName),
                    userId: request.UserId);
                await _notificationRepository.AddAsync(reenableNotif);
                await _unitOfWork.SaveChangesAsync();
                return;
            }

            throw new ConflictException("User Is Already Assigned To This Event");
        }

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

        // Gửi notification cho user được assign — ghi rõ event + role
        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        var eventName = ev?.Name ?? "Event";
        var newAssignNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Assignment",
            string.Format(NotificationMessage.Assignment.AssignedAsRole, request.EventRole, eventName),
            userId: request.UserId);
        await _notificationRepository.AddAsync(newAssignNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignEventRoleToLecturer(AssignEventRoleToLecturerRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var assignEvent = await _assignEventRepository.GetByIdAsync(request.AssignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("User Is Not A Lecturer");

        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid Event Role. Must be: Judge, Mentor");

        if (eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Cannot Assign Staff Role To Lecturer");

        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        assignEvent.EventRoleId = eventRole.Id;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ old role → new role
        var oldRoleName = assignEvent.EventRole?.Name.ToString() ?? "Unknown";
        var roleNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Role Changed",
            string.Format(NotificationMessage.Assignment.RoleChanged, assignEvent.Event?.Name ?? "Event", oldRoleName, request.EventRole),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(roleNotif);
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
                IsDisable = ae.IsDisable,
                AssignTracks = ae.AssignTracks.Select(at => new AssignedTrackItem
                {
                    TrackId = at.TrackId,
                    Title = at.Track.Title,
                    EventId = ae.EventId,
                    IsDisable = at.IsDisable
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

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(request.AssignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        if (assignEvent.EventRole?.Name == Domain.Enums.EventRole.EventRoleEnum.Staff)
            throw new BadRequestException("Staff Cannot Be Assigned To Track");

        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        if (track.EventId != assignEvent.EventId)
            throw new BadRequestException("Track Does Not Belong To The Same Event");

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

        // Gửi notification cho user
        var trackNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Track Assigned",
            string.Format(NotificationMessage.Assignment.AssignedToTrack, track.Title, assignEvent.Event?.Name ?? "Event"),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(trackNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveTrackFromEvent(Guid assignEventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var assignTrack = await _assignEventRepository.GetAssignTrackAsync(assignEventId, trackId);
        if (assignTrack == null)
            throw new NotFoundException("Assign Track Not Found");

        assignTrack.IsDisable = true;
        assignTrack.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.RemoveAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho user
        var removeTrackNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Track Removed",
            string.Format(NotificationMessage.Assignment.RemovedFromTrack, assignTrack.Track?.Title ?? "", assignTrack.AssignEvent?.Event?.Name ?? "Event"),
            userId: assignTrack.AssignEvent?.UserId);
        await _notificationRepository.AddAsync(removeTrackNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreTrackToEvent(Guid assignEventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var assignTrack = await _assignEventRepository.GetAssignTrackAnyAsync(assignEventId, trackId);
        if (assignTrack == null)
            throw new NotFoundException("Assign Track Not Found");

        if (!assignTrack.IsDisable)
            throw new BadRequestException("Assign Track Is Already Active");

        assignTrack.IsDisable = false;
        assignTrack.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.RestoreAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho user
        var restoreTrackNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Track Restored",
            string.Format(NotificationMessage.Assignment.RestoredToTrack, assignTrack.Track?.Title ?? "", assignTrack.AssignEvent?.Event?.Name ?? "Event"),
            userId: assignTrack.AssignEvent?.UserId);
        await _notificationRepository.AddAsync(restoreTrackNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveAssignEvent(Guid assignEventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        if (assignEvent.IsDisable)
            throw new BadRequestException("Assign Event Is Already Removed");

        assignEvent.IsDisable = true;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        // Disable all associated tracks
        foreach (var track in assignEvent.AssignTracks)
        {
            track.IsDisable = true;
            track.UpdatedAt = DateTimeOffset.UtcNow;
        }

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho user — ghi rõ event
        var removedEventName = assignEvent.Event?.Name ?? "Event";
        var removeAssignNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Assignment Removed",
            string.Format(NotificationMessage.Assignment.RemovedFromEvent, removedEventName),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(removeAssignNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreAssignEvent(Guid assignEventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        if (!assignEvent.IsDisable)
            throw new BadRequestException("Assign Event Is Already Active");

        assignEvent.IsDisable = false;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        // Restore all associated tracks
        foreach (var track in assignEvent.AssignTracks)
        {
            track.IsDisable = false;
            track.UpdatedAt = DateTimeOffset.UtcNow;
        }

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho user — ghi rõ event
        var restoredEventName = assignEvent.Event?.Name ?? "Event";
        var restoreAssignNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Assignment Restored",
            string.Format(NotificationMessage.Assignment.RestoredToEvent, restoredEventName),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(restoreAssignNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetUserAssignEventsResponse> GetUserAssignEvents(Guid userId, string? keyword, string? eventRole, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        PaginationHelper.Validate(pageIndex, pageSize);

        // Validate user exists and has valid role
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (user.Role != RoleEnum.Staff && user.Role != RoleEnum.Lecturer)
            throw new BadRequestException("User Must Be Staff Or Lecturer To Have Event Assignments");

        // Parse event role filter
        Domain.Enums.EventRole.EventRoleEnum? eventRoleFilter = null;
        if (!string.IsNullOrWhiteSpace(eventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(eventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Staff, Judge, Mentor");
            eventRoleFilter = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetUserAssignEventsAsync(
            userId, keyword, eventRoleFilter, pageIndex, pageSize);

        return new GetUserAssignEventsResponse
        {
            Events = items.Select(ae => new UserAssignEventItem
            {
                Id = ae.Event.Id,
                Name = ae.Event.Name,
                Description = ae.Event.Description,
                Status = ae.Event.Status?.ToString(),
                NumberRound = ae.Event.NumberRound,
                Season = ae.Event.Season?.ToString(),
                StartTime = ae.Event.StartTime,
                EndTime = ae.Event.EndTime,
                EventRoleId = ae.EventRoleId,
                EventRoleName = ae.EventRole?.Name.ToString(),
                CreatedAt = ae.Event.CreatedAt,
                UpdatedAt = ae.Event.UpdatedAt,
                IsDisable = ae.Event.IsDisable
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
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
