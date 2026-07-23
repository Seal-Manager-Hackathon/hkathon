using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Notification;

public class Service : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRecentNotificationsResponse> GetRecentNotifications()
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var items = await _notificationRepository.GetRecentAsync(10);

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

    public async Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        NotificationTargetTypeEnum? targetType = null;
        if (!string.IsNullOrWhiteSpace(request.TargetType))
        {
            if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var parsed))
                throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");
            targetType = parsed;
        }

        var (items, totalCount) = await _notificationRepository.SearchAsync(
            request.Title, targetType,
            request.FromDate, request.ToDate, request.IsDisable,
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

    public async Task CreateNotification(CreateNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var targetType))
            throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");

        var now = DateTimeOffset.UtcNow;

        if (targetType == NotificationTargetTypeEnum.System)
        {
            var systemNotification = new Notifications
            {
                Id = Guid.NewGuid(),
                UserId = null,
                TeamId = null,
                Title = request.Title,
                Description = request.Description,
                TargetType = NotificationTargetTypeEnum.System,
                Status = NotificationStatusEnum.Unread,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _notificationRepository.AddAsync(systemNotification);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        if (targetType == NotificationTargetTypeEnum.Team)
        {
            if (!request.TeamId.HasValue)
                throw new BadRequestException("TeamId Is Required When TargetType Is Team");

            var team = await _teamRepository.GetByIdAsync(request.TeamId.Value);
            if (team == null)
                throw new NotFoundException("Team Not Found");

            var teamNotification = new Notifications
            {
                Id = Guid.NewGuid(),
                UserId = null,
                TeamId = request.TeamId,
                Title = request.Title,
                Description = request.Description,
                TargetType = NotificationTargetTypeEnum.Team,
                Status = NotificationStatusEnum.Unread,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _notificationRepository.AddAsync(teamNotification);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        // Personal
        if (!request.UserId.HasValue)
            throw new BadRequestException("UserId Is Required When TargetType Is Personal");

        var user = await _userRepository.GetByIdAsync(request.UserId.Value);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        var personalNotification = new Notifications
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            TeamId = null,
            Title = request.Title,
            Description = request.Description,
            TargetType = NotificationTargetTypeEnum.Personal,
            Status = NotificationStatusEnum.Unread,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _notificationRepository.AddAsync(personalNotification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteNotification(DeleteNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification == null)
            throw new NotFoundException("Notification Not Found");

        if (notification.IsDisable)
            throw new BadRequestException("Notification Is Already Disabled");

        notification.IsDisable = true;
        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _notificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreNotification(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null)
            throw new NotFoundException("Notification Not Found");

        notification.IsDisable = false;
        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _notificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateNotification(UpdateNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var notification = await _notificationRepository.GetByIdAsync(request.NotificationId);
        if (notification == null)
            throw new NotFoundException("Notification Not Found");

        if (request.Title != null)
            notification.Title = request.Title;
        if (request.Description != null)
            notification.Description = request.Description;

        notification.UpdatedAt = DateTimeOffset.UtcNow;

        await _notificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<NotificationDetailResponse> GetNotificationDetail(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

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

    public async Task<GetMyNotificationsResponse> GetMyNotifications(GetMyNotificationsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        NotificationTargetTypeEnum? targetType = null;
        if (!string.IsNullOrWhiteSpace(request.TargetType))
        {
            if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var parsed))
                throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");
            targetType = parsed;
        }

        NotificationStatusEnum? status = null;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (!Enum.TryParse<NotificationStatusEnum>(request.Status, true, out var parsed))
                throw new BadRequestException("Invalid Status. Must be: Unread, Read");
            status = parsed;
        }

        var (items, totalCount) = await _notificationRepository.GetUserNotificationsAsync(
            currentUserId.Value, request.Keyword, targetType, status,
            request.FromDate, request.ToDate,
            request.PageIndex, request.PageSize);

        return new GetMyNotificationsResponse
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

    public async Task<NotificationDetailResponse> GetMyNotificationDetail(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

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

    public async Task<GetUnreadCountResponse> GetMyUnreadCount()
    {
        _authorizationService.Authorize(RoleEnum.Staff);

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

    public async Task ReadMyNotification(Guid notificationId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

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

    public async Task ReadAllMyNotifications()
    {
        _authorizationService.Authorize(RoleEnum.Staff);

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
}
