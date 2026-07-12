using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Mentor.MentorNotification;

public class Service : IMentorNotificationService
{
    private readonly IMentorNotificationRepository _mentorNotificationRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        IMentorNotificationRepository mentorNotificationRepository,
        ITrackRepository trackRepository,
        IAssignEventRepository assignEventRepository,
        IEventRepository eventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _mentorNotificationRepository = mentorNotificationRepository;
        _trackRepository = trackRepository;
        _assignEventRepository = assignEventRepository;
        _eventRepository = eventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetMentorTracksResponse> GetTracksByEvent(Guid eventId, int pageIndex = 1, int pageSize = 10)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(pageIndex, pageSize);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Check mentor is assigned to this event
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        // Get all tracks assigned to this mentor in the event
        var tracks = await _trackRepository.GetByEventIdAsync(eventId);
        var mentorTrackIds = assignEvent.AssignTracks
            .Where(at => !at.IsDisable)
            .Select(at => at.TrackId)
            .ToHashSet();

        var filtered = tracks
            .Where(t => mentorTrackIds.Contains(t.Id) && !t.IsDisable)
            .OrderByDescending(t => t.CreatedAt)
            .ToList();

        var totalCount = filtered.Count;
        var items = filtered
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new MentorTrackItem
            {
                Id = t.Id,
                EventId = t.EventId,
                Title = t.Title,
                Description = t.Description,
                MaxTeam = t.MaxTeam,
                IsDisable = t.IsDisable,
                EventRoleId = assignEvent.EventRoleId,
                EventRoleName = assignEvent.EventRole?.Name.ToString(),
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new GetMentorTracksResponse
        {
            Tracks = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<SendNotificationResponse> SendTrackNotification(Guid trackId, SendTrackNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Verify track exists
        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
            throw new NotFoundException("Track Not Found");

        // Verify mentor is assigned to this track in the event
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var assignTrack = assignEvent.AssignTracks
            .FirstOrDefault(at => at.TrackId == trackId && !at.IsDisable);
        if (assignTrack == null)
            throw new ForbiddenException("You Are Not Assigned to This Track");

        var now = DateTimeOffset.UtcNow;
        var notification = new MentorNotifications
        {
            Id = Guid.NewGuid(),
            AssignTrackId = assignTrack.Id,
            Title = request.Title,
            Description = request.Description,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _mentorNotificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();

        return new SendNotificationResponse
        {
            NotificationId = notification.Id,
            Title = notification.Title,
            Description = notification.Description,
            AssignTrackId = notification.AssignTrackId,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<GetNotificationsByTrackResponse> GetTrackNotifications(Guid trackId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        PaginationHelper.Validate(pageIndex, pageSize);

        // Verify track exists
        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
            throw new NotFoundException("Track Not Found");

        // Verify mentor is assigned to this track
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var assignTrack = assignEvent.AssignTracks
            .FirstOrDefault(at => at.TrackId == trackId && !at.IsDisable);
        if (assignTrack == null)
            throw new ForbiddenException("You Are Not Assigned to This Track");

        var totalCount = await _mentorNotificationRepository.CountByAssignTrackIdAsync(assignTrack.Id);
        var items = await _mentorNotificationRepository.GetByAssignTrackIdAsync(assignTrack.Id, pageIndex, pageSize);

        return new GetNotificationsByTrackResponse
        {
            Notifications = items.Select(n => new MentorNotificationItem
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                AssignTrackId = n.AssignTrackId,
                CreatedAt = n.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<MentorNotificationDetailResponse> GetMentorNotificationDetail(Guid mentorNotificationId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var notification = await _mentorNotificationRepository.GetByIdAsync(mentorNotificationId);
        if (notification == null)
            throw new NotFoundException("Mentor Notification Not Found");

        return new MentorNotificationDetailResponse
        {
            Id = notification.Id,
            AssignTrackId = notification.AssignTrackId,
            Title = notification.Title,
            Description = notification.Description,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt
        };
    }

    public async Task UpdateMentorNotification(Guid mentorNotificationId, UpdateMentorNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var notification = await _mentorNotificationRepository.GetByIdAsync(mentorNotificationId);
        if (notification == null)
            throw new NotFoundException("Mentor Notification Not Found");

        if (request.Title != null)
            notification.Title = request.Title;
        if (request.Description != null)
            notification.Description = request.Description;

        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _mentorNotificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMentorNotification(Guid mentorNotificationId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var notification = await _mentorNotificationRepository.GetByIdAsync(mentorNotificationId);
        if (notification == null)
            throw new NotFoundException("Mentor Notification Not Found");

        if (notification.IsDisable)
            throw new BadRequestException("Mentor Notification Is Already Disabled");

        notification.IsDisable = true;
        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _mentorNotificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreMentorNotification(Guid mentorNotificationId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var notification = await _mentorNotificationRepository.GetByIdAsync(mentorNotificationId);
        if (notification == null)
            throw new NotFoundException("Mentor Notification Not Found");

        if (!notification.IsDisable)
            throw new BadRequestException("Mentor Notification Is Already Active");

        notification.IsDisable = false;
        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _mentorNotificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }
}
