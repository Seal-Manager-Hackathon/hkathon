namespace Hackathon.Application.Services.Student.Notification;

public interface INotificationService
{
    Task<GetStudentNotificationsResponse> GetNotifications(GetStudentNotificationsRequest request);
    Task<GetMentorNotificationsResponse> GetMentorNotificationsByRegisterTeam(Guid registerTeamId, int pageIndex, int pageSize);
    Task<StudentMentorNotificationDetailResponse> GetMentorNotificationDetail(Guid mentorNotificationId);
}
