namespace Hackathon.Application.Services.Base.Notification;

public interface INotificationService
{
    Task<GetNotificationDetailResponse> GetNotificationDetail(Guid notificationId);
}
