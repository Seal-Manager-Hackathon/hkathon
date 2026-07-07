using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Notification;

public class Service : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetRecentNotificationsResponse> GetRecentNotifications()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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
                CreatedAt = n.CreatedAt
            }).ToList()
        };
    }

    public async Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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
                CreatedAt = n.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task CreateNotification(CreateNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        if (!Enum.TryParse<NotificationTargetTypeEnum>(request.TargetType, true, out var targetType))
            throw new BadRequestException("Invalid TargetType. Must be: Personal, Team, System");

        var now = DateTimeOffset.UtcNow;

        if (targetType == NotificationTargetTypeEnum.System)
        {
            // Gửi cho tất cả users
            var allUsers = await _userRepository.GetAllAsync();
            var notifications = allUsers.Select(u => new Notifications
            {
                Id = Guid.NewGuid(),
                UserId = u.Id,
                Title = request.Title,
                Description = request.Description,
                TargetType = NotificationTargetTypeEnum.System,
                Status = NotificationStatusEnum.Unread,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            await _notificationRepository.AddRangeAsync(notifications);
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

            // Lấy tất cả thành viên trong team
            var memberIds = await _teamRepository.GetTeamMemberIdsAsync(request.TeamId.Value);
            var notifications = memberIds.Select(userId => new Notifications
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TeamId = request.TeamId,
                Title = request.Title,
                Description = request.Description,
                TargetType = NotificationTargetTypeEnum.Team,
                Status = NotificationStatusEnum.Unread,
                CreatedAt = now,
                UpdatedAt = now
            }).ToList();

            await _notificationRepository.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        // Personal — gửi cho 1 người
        if (!request.UserId.HasValue)
            throw new BadRequestException("UserId Is Required When TargetType Is Personal");

        var user = await _userRepository.GetByIdAsync(request.UserId.Value);
        if (user == null)
            throw new NotFoundException(ErrMsg.Auth.UserNotFound);

        // Nếu vừa có UserId vừa có TeamId → check user có trong team không
        if (request.TeamId.HasValue)
        {
            var isInTeam = await _teamRepository.IsUserInTeamAsync(request.TeamId.Value, request.UserId.Value);
            if (!isInTeam)
                throw new BadRequestException("User Is Not In The Specified Team");
        }

        var notification = new Notifications
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            TeamId = request.TeamId,
            Title = request.Title,
            Description = request.Description,
            TargetType = NotificationTargetTypeEnum.Personal,
            Status = NotificationStatusEnum.Unread,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteNotification(DeleteNotificationRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

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
        _authorizationService.Authorize(RoleEnum.Admin);

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
        _authorizationService.Authorize(RoleEnum.Admin);

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
        _authorizationService.Authorize(RoleEnum.Admin);

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
