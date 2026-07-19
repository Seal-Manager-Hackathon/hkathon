using Hackathon.Application.Common;
using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Notification;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.User;
using Hackathon.Domain.Enums.EventRole;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Assign;

public class Service : IAssignService
{
    private readonly IUserRepository _userRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IUserRepository userRepository,
        IAssignEventRepository assignEventRepository,
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _userRepository = userRepository;
        _assignEventRepository = assignEventRepository;
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetAvailableStaffResponse> GetAvailableStaff(Guid eventId, GetAvailableLecturersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var (items, totalCount) = await _userRepository.GetAvailableUsersByRoleAsync(
            eventId, RoleEnum.Staff, request.Keyword, request.PageIndex, request.PageSize);

        return new GetAvailableStaffResponse
        {
            Items = items.Select(u => new AvailableStaffItem
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                AvatarUrl = string.IsNullOrEmpty(u.AvatarUrl) ? null : u.AvatarUrl,
                PhoneNumber = string.IsNullOrEmpty(u.PhoneNumber) ? null : u.PhoneNumber
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetAvailableLecturersResponse> GetAvailableLecturers(Guid eventId, GetAvailableLecturersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var (items, totalCount) = await _userRepository.GetAvailableUsersByRoleAsync(
            eventId, RoleEnum.Lecturer, request.Keyword, request.PageIndex, request.PageSize);

        return new GetAvailableLecturersResponse
        {
            Items = items.Select(u => new AvailableLecturerItem
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

    public async Task AssignLecturerToEvent(Guid eventId, AssignLecturerToEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (user.IsDisable)
            throw new BadRequestException("Cannot Assign A Disabled User");
        if (!string.IsNullOrEmpty(user.BanReason))
            throw new BadRequestException("Cannot Assign A Banned User");

        if (user.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Assign Lecturer To Event");

        if (!Enum.TryParse<EventRoleEnum>(request.EventRole, true, out var eventRoleEnum))
            throw new BadRequestException("Invalid EventRole. Must be: Judge, Mentor");

        if (eventRoleEnum == EventRoleEnum.Staff)
            throw new BadRequestException("Staff Cannot Assign Staff Role");

        var eventRole = await _assignEventRepository.GetEventRoleByNameAsync(eventRoleEnum);
        if (eventRole == null)
            throw new NotFoundException($"Event Role {request.EventRole} Not Found");

        var existing = await _assignEventRepository.GetByEventIdAndUserIdAsync(eventId, request.UserId);

        if (existing != null)
        {
            if (existing.IsDisable)
            {
                // Re-enable + update role
                existing.IsDisable = false;
                existing.EventRoleId = eventRole.Id;
                existing.UpdatedAt = DateTimeOffset.UtcNow;
                await _unitOfWork.SaveChangesAsync();
                return;
            }

            throw new ConflictException("User Is Already Assigned To This Event");
        }

        var assignEvent = new Hackathon.Domain.Entities.AssignEvents
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            UserId = request.UserId,
            EventRoleId = eventRole.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _assignEventRepository.Add(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ event + role
        var ev = await _eventRepository.GetByIdAsync(eventId);
        var eventName = ev?.Name ?? "Event";
        var assignNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Assignment",
            string.Format(NotificationMessage.Assignment.AssignedAsRole, request.EventRole, eventName),
            userId: request.UserId);
        await _notificationRepository.AddAsync(assignNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);

        Domain.Enums.EventRole.EventRoleEnum? eventRole = null;
        if (!string.IsNullOrWhiteSpace(request.EventRole))
        {
            if (!Enum.TryParse<Domain.Enums.EventRole.EventRoleEnum>(request.EventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Mentor, Judge, Staff");
            eventRole = parsed;
        }

        Domain.Enums.User.RoleEnum? role = null;
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            if (!Enum.TryParse<Domain.Enums.User.RoleEnum>(request.Role, true, out var parsed))
                throw new BadRequestException("Invalid Role. Must be: Admin, Staff, Student, Lecturer");
            role = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetAssignedUsersByEventAsync(
            eventId, request.Keyword, eventRole, role, request.TrackId,
            request.PageIndex, request.PageSize);

        return new GetAssignedUsersResponse
        {
            Items = items
                .Where(ae => !ae.IsDisable)
                .Select(ae => new AssignedUserItem
                {
                    AssignEventId = ae.Id,
                    UserId = ae.User.Id,
                    Email = ae.User.Email,
                    FirstName = ae.User.FirstName,
                    LastName = ae.User.LastName,
                    AvatarUrl = string.IsNullOrEmpty(ae.User.AvatarUrl) ? null : ae.User.AvatarUrl,
                    EventRole = ae.EventRole?.Name.ToString(),
                    IsDisable = ae.IsDisable,
                    AssignTracks = ae.AssignTracks
                        .Select(at => new AssignedTrackItem
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

    public async Task<GetAssignEventDetailResponse> GetAssignEventDetail(Guid assignEventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null || assignEvent.IsDisable)
            throw new NotFoundException("Assign Event Not Found");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        return new GetAssignEventDetailResponse
        {
            Id = assignEvent.Id,
            EventId = assignEvent.EventId,
            UserId = assignEvent.User.Id,
            Email = assignEvent.User.Email,
            FirstName = assignEvent.User.FirstName,
            LastName = assignEvent.User.LastName,
            AvatarUrl = string.IsNullOrEmpty(assignEvent.User.AvatarUrl) ? null : assignEvent.User.AvatarUrl,
            EventRole = assignEvent.EventRole?.Name.ToString(),
            Tracks = assignEvent.AssignTracks
                .Select(at => new AssignedTrackItem
                {
                    TrackId = at.TrackId,
                    Title = at.Track.Title,
                    EventId = assignEvent.EventId,
                    IsDisable = at.IsDisable
                }).ToList(),
            CreatedAt = assignEvent.CreatedAt,
            UpdatedAt = assignEvent.UpdatedAt
        };
    }

    public async Task RemoveAssignEvent(Guid assignEventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff can only affect Lecturers — cannot remove Staff from event
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Remove Lecturer From Event");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        if (assignEvent.IsDisable)
            throw new BadRequestException("Assign Event Is Already Removed");

        assignEvent.IsDisable = true;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        // Cascade soft-delete all associated tracks
        foreach (var track in assignEvent.AssignTracks)
        {
            track.IsDisable = true;
            track.UpdatedAt = DateTimeOffset.UtcNow;
        }

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ event
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
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff can only affect Lecturers — cannot restore Staff
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Restore Lecturer");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        if (!assignEvent.IsDisable)
            throw new BadRequestException("Assign Event Is Already Active");

        assignEvent.IsDisable = false;
        assignEvent.UpdatedAt = DateTimeOffset.UtcNow;

        // Cascade restore all associated tracks
        foreach (var track in assignEvent.AssignTracks)
        {
            track.IsDisable = false;
            track.UpdatedAt = DateTimeOffset.UtcNow;
        }

        _assignEventRepository.Update(assignEvent);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ event
        var restoredEventName = assignEvent.Event?.Name ?? "Event";
        var restoreAssignNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Event Assignment Restored",
            string.Format(NotificationMessage.Assignment.RestoredToEvent, restoredEventName),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(restoreAssignNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AssignTrackToEvent(Guid assignEventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff can only assign tracks to Lecturers
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Assign Track To Lecturer");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        if (track.EventId != assignEvent.EventId)
            throw new BadRequestException("Track Does Not Belong To The Same Event");

        var isAssigned = await _assignEventRepository.IsTrackAssignedAsync(assignEventId, trackId);
        if (isAssigned)
            throw new ConflictException("Track Is Already Assigned To This User");

        var assignTrack = new Hackathon.Domain.Entities.AssignTracks
        {
            Id = Guid.NewGuid(),
            AssignEventId = assignEventId,
            TrackId = trackId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        _assignEventRepository.AddAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ track + event
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
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff can only affect Lecturers' tracks
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Modify Lecturer's Tracks");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        var assignTrack = await _assignEventRepository.GetAssignTrackAsync(assignEventId, trackId);
        if (assignTrack == null)
            throw new NotFoundException("Assign Track Not Found");

        assignTrack.IsDisable = true;
        assignTrack.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.RemoveAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ track + event
        var removeTrackNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Track Removed",
            string.Format(NotificationMessage.Assignment.RemovedFromTrack, assignTrack.Track?.Title ?? "", assignEvent.Event?.Name ?? "Event"),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(removeTrackNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreTrackToEvent(Guid assignEventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var assignEvent = await _assignEventRepository.GetByIdWithTracksAsync(assignEventId);
        if (assignEvent == null)
            throw new NotFoundException("Assign Event Not Found");

        // Staff can only affect Lecturers' tracks
        if (assignEvent.User.Role != RoleEnum.Lecturer)
            throw new BadRequestException("Can Only Modify Lecturer's Tracks");

        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, assignEvent.EventId);

        var assignTrack = await _assignEventRepository.GetAssignTrackAnyAsync(assignEventId, trackId);
        if (assignTrack == null)
            throw new NotFoundException("Assign Track Not Found");

        if (!assignTrack.IsDisable)
            throw new BadRequestException("Assign Track Is Already Active");

        assignTrack.IsDisable = false;
        assignTrack.UpdatedAt = DateTimeOffset.UtcNow;

        _assignEventRepository.RestoreAssignTrack(assignTrack);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho lecturer — ghi rõ track + event
        var restoreTrackNotif = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Track Restored",
            string.Format(NotificationMessage.Assignment.RestoredToTrack, assignTrack.Track?.Title ?? "", assignEvent.Event?.Name ?? "Event"),
            userId: assignEvent.UserId);
        await _notificationRepository.AddAsync(restoreTrackNotif);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<Admin.Assign.GetUserAssignEventsResponse> GetUserAssignEvents(Guid userId, string? keyword, string? eventRole, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        // Staff must be assigned to at least one event to use this
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (currentAssigns, _) = await _assignEventRepository.GetStaffAssignEventsByUserIdAsync(
            currentUserId.Value, null, null, null, null, 1, 1);
        if (currentAssigns.Count == 0)
            throw new ForbiddenException("You Are Not Assigned to Any Event");

        PaginationHelper.Validate(pageIndex, pageSize);

        // Validate target user
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        if (user.Role != RoleEnum.Staff && user.Role != RoleEnum.Lecturer)
            throw new BadRequestException("User Must Be Staff Or Lecturer To Have Event Assignments");

        // Parse event role filter
        EventRoleEnum? eventRoleFilter = null;
        if (!string.IsNullOrWhiteSpace(eventRole))
        {
            if (!Enum.TryParse<EventRoleEnum>(eventRole, true, out var parsed))
                throw new BadRequestException("Invalid EventRole. Must be: Staff, Judge, Mentor");
            eventRoleFilter = parsed;
        }

        var (items, totalCount) = await _assignEventRepository.GetUserAssignEventsAsync(
            userId, keyword, eventRoleFilter, pageIndex, pageSize);

        return new Admin.Assign.GetUserAssignEventsResponse
        {
            Events = items.Select(ae => new Admin.Assign.UserAssignEventItem
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
}
