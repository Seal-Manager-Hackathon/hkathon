using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.RegisterTeam;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Notification;

public class Service : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IMentorNotificationRepository _mentorNotificationRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        INotificationRepository notificationRepository,
        ITeamRepository teamRepository,
        ICurrentUserService currentUserService,
        IRegisterTeamRepository registerTeamRepository,
        IMentorNotificationRepository mentorNotificationRepository,
        IAssignEventRepository assignEventRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _registerTeamRepository = registerTeamRepository;
        _mentorNotificationRepository = mentorNotificationRepository;
        _assignEventRepository = assignEventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetStudentNotificationsResponse> GetNotifications(GetStudentNotificationsRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        // Parse target type
        NotificationTargetTypeEnum? targetType = null;
        if (!string.IsNullOrWhiteSpace(request.TargetType))
        {
            if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var parsed))
                throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");
            targetType = parsed;
        }

        // Parse status
        NotificationStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<NotificationStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Unread, Read");
            status = parsed;
        }

        // Get active team ids (team ko disable, team detail active + ko disable)
        var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);

        var (items, totalCount) = await _notificationRepository.GetStudentNotificationsAsync(
            userId, teamIds, request.Keyword, targetType, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetStudentNotificationsResponse
        {
            Notifications = items.Select(n => new StudentNotificationItem
            {
                Id = n.Id,
                UserId = n.UserId,
                TeamId = n.TeamId,
                Title = n.Title,
                Status = n.Status?.ToString(),
                Description = n.Description,
                TargetType = n.TargetType.ToString(),
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetMentorNotificationsResponse> GetMentorNotificationsByRegisterTeam(Guid registerTeamId, int pageIndex, int pageSize)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        PaginationHelper.Validate(pageIndex, pageSize);

        var rt = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (rt == null || rt.IsDisable)
            throw new NotFoundException("Register Team Not Found");

        if (rt.Status != RegisterTeamStatusEnum.Approved)
            throw new BadRequestException("Only Approved Register Team Can View Mentor Notifications");

        // Check user belongs to this team
        var members = await _teamRepository.GetTeamMembersAsync(rt.TeamId);
        var isMember = members.Any(m => m.UserId == userId && !m.IsDisable);
        if (!isMember)
            throw new ForbiddenException("You Are Not a Member of This Team");

        // Lấy tất cả AssignTracks có TrackId trùng với register team's track trong cùng event
        var assignTrackIds = new List<Guid>();
        if (rt.TrackId.HasValue)
        {
            var assignTracks = await _assignEventRepository.GetAssignTracksByTrackIdAsync(rt.EventId, rt.TrackId.Value);
            assignTrackIds = assignTracks.Select(at => at.Id).ToList();
        }

        if (assignTrackIds.Count == 0)
        {
            return new GetMentorNotificationsResponse
            {
                Notifications = new List<StudentMentorNotificationItem>(),
                TotalCount = 0,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        var (items, totalCount) = await _mentorNotificationRepository.GetByAssignTrackIdsAsync(assignTrackIds, pageIndex, pageSize);

        return new GetMentorNotificationsResponse
        {
            Notifications = items.Select(n => new StudentMentorNotificationItem
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                MentorFirstName = n.AssignTrack?.AssignEvent?.User?.FirstName,
                MentorLastName = n.AssignTrack?.AssignEvent?.User?.LastName,
                MentorEmail = n.AssignTrack?.AssignEvent?.User?.Email,
                MentorAvatarUrl = n.AssignTrack?.AssignEvent?.User?.AvatarUrl,
                TrackTitle = n.AssignTrack?.Track?.Title,
                CreatedAt = n.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<StudentMentorNotificationDetailResponse> GetMentorNotificationDetail(Guid mentorNotificationId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var notification = await _mentorNotificationRepository.GetByIdAsync(mentorNotificationId);
        if (notification == null || notification.IsDisable)
            throw new NotFoundException("Mentor Notification Not Found");

        // Verify user is a member of a team that belongs to this notification's track
        var userTeamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);
        var hasAccess = false;
        if (notification.AssignTrack != null)
        {
            foreach (var teamId in userTeamIds)
            {
                var (items, _) = await _registerTeamRepository.GetByTeamIdAsync(
                    teamId, RegisterTeamStatusEnum.Approved, false, 1, 1);
                if (items.Any(rt => rt.TrackId == notification.AssignTrack.TrackId))
                {
                    hasAccess = true;
                    break;
                }
            }
        }

        if (!hasAccess)
            throw new ForbiddenException("You Do Not Have Access to This Mentor Notification");

        var mentor = notification.AssignTrack?.AssignEvent?.User;
        var mentorRole = notification.AssignTrack?.AssignEvent?.EventRole?.Name.ToString();

        return new StudentMentorNotificationDetailResponse
        {
            Id = notification.Id,
            Title = notification.Title,
            Description = notification.Description,
            MentorUserId = mentor?.Id,
            MentorFirstName = mentor?.FirstName,
            MentorLastName = mentor?.LastName,
            MentorEmail = mentor?.Email,
            MentorAvatarUrl = mentor?.AvatarUrl,
            MentorRole = mentorRole,
            EventId = notification.AssignTrack?.AssignEvent?.EventId,
            EventName = notification.AssignTrack?.AssignEvent?.Event?.Name,
            TrackId = notification.AssignTrack?.TrackId,
            TrackTitle = notification.AssignTrack?.Track?.Title,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt
        };
    }

    public async Task<StudentNotificationDetailResponse> GetNotificationDetail(Guid notificationId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var notification = await _notificationRepository.GetDetailByIdAsync(notificationId);
        if (notification == null || notification.IsDisable)
            throw new NotFoundException("Notification Not Found");

        // Verify access: only owner, team member (for Team notifications), or system notification
        var hasAccess = notification.UserId == userId
            || notification.TargetType == Domain.Enums.Notification.NotificationTargetTypeEnum.System;

        if (!hasAccess && notification.TargetType == Domain.Enums.Notification.NotificationTargetTypeEnum.Team
            && notification.TeamId.HasValue)
        {
            var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);
            hasAccess = teamIds.Contains(notification.TeamId.Value);
        }

        if (!hasAccess)
            throw new ForbiddenException("You Do Not Have Access to This Notification");

        return new StudentNotificationDetailResponse
        {
            Id = notification.Id,
            UserId = notification.UserId,
            TeamId = notification.TeamId,
            Title = notification.Title,
            Status = notification.Status?.ToString(),
            Description = notification.Description,
            TargetType = notification.TargetType.ToString(),
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt
        };
    }

    public async Task<NotificationCountResponse> GetNotificationCount(string? status)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        // Parse status filter
        NotificationStatusEnum? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<NotificationStatusEnum>(status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Pending, Unread, Read");
            statusFilter = parsed;
        }

        var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);

        var totalCount = await _notificationRepository.CountStudentNotificationsAsync(
            userId, teamIds, null, null, statusFilter, null, null);

        return new NotificationCountResponse { Total = totalCount };
    }

    public async Task<GetUnreadCountResponse> GetMyUnreadCount()
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);

        var count = await _notificationRepository.CountStudentNotificationsAsync(
            userId, teamIds, null, null, NotificationStatusEnum.Unread, null, null);

        return new GetUnreadCountResponse { Count = count };
    }

    public async Task ReadNotification(Guid notificationId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var notification = await _notificationRepository.GetDetailByIdAsync(notificationId);
        if (notification == null || notification.IsDisable)
            throw new NotFoundException("Notification Not Found");

        // Check access
        var hasAccess = notification.UserId == userId
            || notification.TargetType == Domain.Enums.Notification.NotificationTargetTypeEnum.System;

        if (!hasAccess && notification.TargetType == Domain.Enums.Notification.NotificationTargetTypeEnum.Team
            && notification.TeamId.HasValue)
        {
            var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);
            hasAccess = teamIds.Contains(notification.TeamId.Value);
        }

        if (!hasAccess)
            throw new ForbiddenException("You Do Not Have Access to This Notification");

        if (notification.Status != NotificationStatusEnum.Read)
        {
            notification.Status = NotificationStatusEnum.Read;
            notification.UpdatedAt = DateTimeOffset.UtcNow;
            await _notificationRepository.UpdateAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task ReadAllNotifications()
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var teamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);

        // Lấy tất cả notification chưa đọc (Unread hoặc Pending) của user
        var (items, _) = await _notificationRepository.GetStudentNotificationsAsync(
            userId, teamIds, null, null, NotificationStatusEnum.Unread, null, null, 1, int.MaxValue);

        if (items.Count == 0)
            return;

        var now = DateTimeOffset.UtcNow;
        foreach (var n in items)
        {
            n.Status = NotificationStatusEnum.Read;
            n.UpdatedAt = now;
        }

        await _notificationRepository.UpdateRangeAsync(items);
        await _unitOfWork.SaveChangesAsync();
    }
}
