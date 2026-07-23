using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Notification;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Notification;

public class Service : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Parse TargetType
        NotificationTargetTypeEnum? targetType = null;
        if (!string.IsNullOrWhiteSpace(request.TargetType))
        {
            if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var parsed))
                throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");
            targetType = parsed;
        }

        // GetUserNotificationsAsync tự filter:
        //   - IsDisable = false (luôn)
        //   - Personal: UserId == currentUser
        //   - System: TargetType == System
        var (items, totalCount) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, request.Title, targetType, null,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetNotificationsResponse
        {
            Notifications = items.Select(n => new NotificationCard
            {
                Id = n.Id,
                UserId = n.UserId,
                TeamId = n.TeamId,
                Title = n.Title,
                Status = n.Status?.ToString(),
                Description = n.Description,
                TargetType = n.TargetType.ToString(),
                IsDisable = n.IsDisable,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetRecentNotificationsResponse> GetRecentNotifications()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, _) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, null, null, null,
            null, null, 1, 10);

        return new GetRecentNotificationsResponse
        {
            Notifications = items.Select(n => new NotificationCard
            {
                Id = n.Id,
                UserId = n.UserId,
                TeamId = n.TeamId,
                Title = n.Title,
                Status = n.Status?.ToString(),
                Description = n.Description,
                TargetType = n.TargetType.ToString(),
                IsDisable = n.IsDisable,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            }).ToList()
        };
    }

    public async Task<GetNotificationCountResponse> GetNotificationCount()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, _) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, null, null, null,
            null, null, 1, int.MaxValue);

        return new GetNotificationCountResponse
        {
            Total = items.Count
        };
    }

    public async Task<GetUnreadCountResponse> GetMyUnreadCount()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, _) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, null, null, NotificationStatusEnum.Unread,
            null, null, 1, int.MaxValue);

        return new GetUnreadCountResponse
        {
            Count = items.Count
        };
    }

    public async Task ReadNotification(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            throw new NotFoundException("Notification Not Found");

        if (notification.TargetType != NotificationTargetTypeEnum.System
            && notification.TargetType != NotificationTargetTypeEnum.Team
            && notification.UserId != currentUserId.Value)
            throw new ForbiddenException("You Do Not Have Access to This Notification");

        notification.Status = NotificationStatusEnum.Read;
        notification.UpdatedAt = DateTimeOffset.UtcNow;
        await _notificationRepository.UpdateAsync(notification);
    }

    public async Task ReadAllNotifications()
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var (items, _) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, null, null, NotificationStatusEnum.Unread,
            null, null, 1, int.MaxValue);

        var now = DateTimeOffset.UtcNow;
        foreach (var notification in items)
        {
            notification.Status = NotificationStatusEnum.Read;
            notification.UpdatedAt = now;
            await _notificationRepository.UpdateAsync(notification);
        }
    }

    public async Task<NotificationDetailResponse> GetNotificationDetail(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            throw new NotFoundException("Notification Not Found");

        return new NotificationDetailResponse
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
}