using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;

namespace Hackathon.Application.Common.Helpers.Notification;

/// <summary>
/// Helper tạo thông báo với target type linh hoạt (Personal / Team / System).
/// Status tự động set là Unread.
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    /// Tạo notification — target type quyết định gán UserId, TeamId hay cả 2 là null.
    /// </summary>
    public static Notifications Create(
        NotificationTargetTypeEnum targetType,
        string? title,
        string? description,
        Guid? userId = null,
        Guid? teamId = null)
    {
        var notification = new Notifications
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            TargetType = targetType,
            Status = NotificationStatusEnum.Unread,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        switch (targetType)
        {
            case NotificationTargetTypeEnum.Personal:
                notification.UserId = userId;
                break;
            case NotificationTargetTypeEnum.Team:
                notification.TeamId = teamId;
                break;
            case NotificationTargetTypeEnum.System:
                // không gán UserId hay TeamId
                break;
        }

        return notification;
    }

    /// <summary>
    /// Filter danh sách notification thuộc về 1 user:
    /// 1. Personal của chính user đó (UserId == userId)
    /// 2. Team của team mà user tham gia (TeamId có trong userTeamIds)
    /// 3. System của toàn hệ thống
    /// </summary>
    public static IQueryable<Notifications> GetUserNotificationsQuery(
        IQueryable<Notifications> query,
        Guid userId,
        List<Guid> userTeamIds)
    {
        return query.Where(n => !n.IsDisable && (
            n.UserId == userId ||
            (n.TeamId.HasValue && userTeamIds.Contains(n.TeamId.Value)) ||
            n.TargetType == NotificationTargetTypeEnum.System
        ));
    }
}
