using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Notification;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Notification;

public class Service : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        INotificationRepository notificationRepository,
        ITeamRepository teamRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _notificationRepository = notificationRepository;
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetNotificationDetailResponse> GetNotificationDetail(Guid notificationId)
    {
        // 1. Check Authenticate (yêu cầu đăng nhập)
        _authorizationService.Authenticate();

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // 2. Lấy thông báo
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification == null || notification.IsDisable)
            throw new NotFoundException("Notification Not Found");

        // 3. Phân quyền truy cập
        if (notification.TargetType == NotificationTargetTypeEnum.System)
        {
            // System notification: Ai cũng xem được
        }
        else if (notification.TargetType == NotificationTargetTypeEnum.Personal)
        {
            // Personal notification: Phải khớp UserId
            if (notification.UserId != currentUserId.Value)
                throw new ForbiddenException("You do not have permission to view this notification");
        }
        else if (notification.TargetType == NotificationTargetTypeEnum.Team)
        {
            // Team notification: User phải nằm trong Team đó
            if (notification.TeamId.HasValue)
            {
                var userTeamIds = await _teamRepository.GetUserTeamIdsAsync(currentUserId.Value);
                if (!userTeamIds.Contains(notification.TeamId.Value))
                    throw new ForbiddenException("You do not have permission to view this team's notification");
            }
            else
            {
                throw new ForbiddenException("You do not have permission to view this notification");
            }
        }
        else
        {
            throw new ForbiddenException("You do not have permission to view this notification");
        }

        return new GetNotificationDetailResponse
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
