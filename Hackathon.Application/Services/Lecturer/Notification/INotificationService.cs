namespace Hackathon.Application.Services.Lecturer.Notification;

public interface INotificationService
{
    Task<GetMyNotificationsResponse> GetMyNotifications(GetMyNotificationsRequest request);
    Task<NotificationDetailResponse> GetMyNotificationDetail(Guid notificationId);
    Task<GetUnreadCountResponse> GetMyUnreadCount();
    Task ReadNotification(Guid notificationId);
    Task ReadAllNotifications();
}
