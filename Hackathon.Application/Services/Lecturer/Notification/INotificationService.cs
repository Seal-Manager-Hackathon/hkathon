namespace Hackathon.Application.Services.Lecturer.Notification;

public interface INotificationService
{
    Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request);
    Task<GetRecentNotificationsResponse> GetRecentNotifications();
    Task<GetNotificationCountResponse> GetNotificationCount();
    Task<NotificationDetailResponse> GetNotificationDetail(Guid notificationId);
    Task<GetUnreadCountResponse> GetMyUnreadCount();
    Task ReadNotification(Guid notificationId);
    Task ReadAllNotifications();
}