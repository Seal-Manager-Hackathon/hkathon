namespace Hackathon.Application.Services.Staff.Notification;

public interface INotificationService
{
    Task<GetNotificationsResponse> GetNotifications(GetNotificationsRequest request);
    Task<NotificationDetailResponse> GetNotificationDetail(Guid notificationId);
    Task<GetRecentNotificationsResponse> GetRecentNotifications();
    Task CreateNotification(CreateNotificationRequest request);
    Task UpdateNotification(UpdateNotificationRequest request);
    Task DeleteNotification(DeleteNotificationRequest request);
    Task RestoreNotification(Guid notificationId);
}
