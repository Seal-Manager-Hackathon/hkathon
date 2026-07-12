using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.TeamDetail;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Notification;

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

    public async Task<GetStudentNotificationsResponse> GetNotifications(GetStudentNotificationsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Student);

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
}
