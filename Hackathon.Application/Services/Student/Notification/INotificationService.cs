namespace Hackathon.Application.Services.Student.Notification;

public interface INotificationService
{
    Task<GetStudentNotificationsResponse> GetNotifications(GetStudentNotificationsRequest request);
    Task<GetMentorNotificationsResponse> GetMentorNotificationsByRegisterTeam(Guid registerTeamId, int pageIndex, int pageSize);
    Task<StudentMentorNotificationDetailResponse> GetMentorNotificationDetail(Guid mentorNotificationId);
    Task<StudentNotificationDetailResponse> GetNotificationDetail(Guid notificationId);
    Task<NotificationCountResponse> GetNotificationCount(string? status);
    Task<GetUnreadCountResponse> GetMyUnreadCount();
    Task ReadNotification(Guid notificationId);
    Task ReadAllNotifications();
}
