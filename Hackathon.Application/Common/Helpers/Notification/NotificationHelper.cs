using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;

namespace Hackathon.Application.Common.Helpers.Notification;

/// <summary>
/// Helper tạo thông báo với các loại target khác nhau (Personal, Team, System).
/// Status tự động set là Pending (chờ xử lý).
/// </summary>
public static class NotificationHelper
{
    /// <summary>
    /// Tạo thông báo Personal — gửi đến 1 người dùng cụ thể.
    /// </summary>
    public static Notifications CreatePersonal(Guid userId, string? title, string? description)
    {
        return new Notifications
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Description = description,
            TargetType = NotificationTargetTypeEnum.Personal,
            Status = NotificationStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Tạo thông báo Team — gửi đến tất cả thành viên trong team.
    /// </summary>
    public static Notifications CreateTeam(Guid teamId, string? title, string? description)
    {
        return new Notifications
        {
            Id = Guid.NewGuid(),
            TeamId = teamId,
            Title = title,
            Description = description,
            TargetType = NotificationTargetTypeEnum.Team,
            Status = NotificationStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Tạo thông báo System — gửi đến tất cả người dùng.
    /// </summary>
    public static Notifications CreateSystem(string? title, string? description)
    {
        return new Notifications
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            TargetType = NotificationTargetTypeEnum.System,
            Status = NotificationStatusEnum.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    /// Tạo thông báo với target type linh hoạt — dùng khi cần switch theo enum.
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
            Status = NotificationStatusEnum.Pending,
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
                break;
        }

        return notification;
    }

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
