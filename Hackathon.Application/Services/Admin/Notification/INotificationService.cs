namespace Hackathon.Application.Services.Admin.Notification;

public interface INotificationService
{
    Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request);
    Task<NotificationDetailResponse> GetNotificationDetail(Guid notificationId);
    Task<GetRecentNotificationsResponse> GetRecentNotifications();
    Task CreateNotification(CreateNotificationRequest request);
    Task UpdateNotification(UpdateNotificationRequest request);
    Task DeleteNotification(DeleteNotificationRequest request);
    Task RestoreNotification(Guid notificationId);
    // My notifications (Personal + System)
    Task<GetMyNotificationsResponse> GetMyNotifications(GetMyNotificationsRequest request);
    Task<NotificationDetailResponse> GetMyNotificationDetail(Guid notificationId);
    Task<GetUnreadCountResponse> GetMyUnreadCount();
    Task ReadNotification(Guid notificationId);
    Task ReadAllNotifications();
}
