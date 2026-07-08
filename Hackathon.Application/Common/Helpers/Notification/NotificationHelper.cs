using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;

namespace Hackathon.Application.Common.Helpers.Notification;

/// <summary>
/// Helper xử lý filter notifications cho user cụ thể.
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    /// Filter danh sách notification thuộc về 1 user:
    /// 1. Personal notification của chính user đó (UserId == userId)
    /// 2. Team notification của team mà user tham gia (TeamId có trong userTeamIds)
    /// 3. System notification của toàn hệ thống
    /// </summary>
    public static IQueryable<Notifications> GetUserNotificationsQuery(
        IQueryable<Notifications> notificationsQuery,
        Guid userId,
        List<Guid> userTeamIds)
    {
        return notificationsQuery.Where(n => !n.IsDisable && (
            n.UserId == userId ||
            (n.TeamId.HasValue && userTeamIds.Contains(n.TeamId.Value)) ||
            n.TargetType == NotificationTargetTypeEnum.System
        ));
    }

    /// <summary>
    /// Filter danh sách notification dạng Team của các team mà user tham gia:
    /// - TargetType == Team
    /// - TeamId có trong danh sách userTeamIds
    /// </summary>
    public static IQueryable<Notifications> GetUserTeamNotificationsQuery(
        IQueryable<Notifications> notificationsQuery,
        List<Guid> userTeamIds)
    {
        return notificationsQuery.Where(n => !n.IsDisable
            && n.TargetType == NotificationTargetTypeEnum.Team
            && n.TeamId.HasValue
            && userTeamIds.Contains(n.TeamId.Value));
    }

    /// <summary>
    /// Filter danh sách notification dạng System:
    /// - TargetType == System
    /// </summary>
    public static IQueryable<Notifications> GetSystemNotificationsQuery(
        IQueryable<Notifications> notificationsQuery)
    {
        return notificationsQuery.Where(n => !n.IsDisable
            && n.TargetType == NotificationTargetTypeEnum.System);
    }
}
