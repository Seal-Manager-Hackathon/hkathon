namespace Hackathon.Application.Services.Mentor.MentorNotification;

public interface IMentorNotificationService
{
    Task<List<MentorTrackItem>> GetTracksByEvent(Guid eventId);
    Task<SendNotificationResponse> SendTrackNotification(Guid trackId, SendTrackNotificationRequest request);
    Task<GetNotificationsByTrackResponse> GetTrackNotifications(Guid trackId, int pageIndex, int pageSize);
    Task<MentorNotificationDetailResponse> GetMentorNotificationDetail(Guid mentorNotificationId);
    Task UpdateMentorNotification(Guid mentorNotificationId, UpdateMentorNotificationRequest request);
    Task DeleteMentorNotification(Guid mentorNotificationId);
    Task RestoreMentorNotification(Guid mentorNotificationId);
}
