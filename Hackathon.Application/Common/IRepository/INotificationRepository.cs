using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface INotificationRepository
{
    Task<Notifications?> GetByIdAsync(Guid id);
    Task AddAsync(Notifications notification);
    Task AddRangeAsync(List<Notifications> notifications);
    Task UpdateAsync(Notifications notification);
    Task<List<Notifications>> GetRecentAsync(int count);
    Task<(List<Notifications> Items, int TotalCount)> SearchAsync(
        string? title, Domain.Enums.Notification.NotificationTargetTypeEnum? targetType,
        DateTimeOffset? fromDate, DateTimeOffset? toDate, bool? isDisable,
        int pageIndex, int pageSize);
    Task<(List<Notifications> Items, int TotalCount)> GetUserNotificationsAsync(
        Guid userId, string? keyword, Domain.Enums.Notification.NotificationTargetTypeEnum? targetType,
        Domain.Enums.Notification.NotificationStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
    Task<(List<Notifications> Items, int TotalCount)> GetStudentNotificationsAsync(
        Guid userId, List<Guid> teamIds, string? keyword,
        Domain.Enums.Notification.NotificationTargetTypeEnum? targetType,
        Domain.Enums.Notification.NotificationStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
}
