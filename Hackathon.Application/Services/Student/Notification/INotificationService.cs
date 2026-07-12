namespace Hackathon.Application.Services.Student.Notification;

public interface INotificationService
{
    Task<GetStudentNotificationsResponse> GetNotifications(GetStudentNotificationsRequest request);
}
