using Hackathon.Service.Models;

namespace Hackathon.Service.Notifications;

public interface IService
{
    // #{All authenticated}
    Task<BasePaginationResponse> GetMyNotifications(PaginationRequest paginationRequest);
    Task<int> GetUnreadCount();
    Task<string> MarkAsRead(Guid notificationId);
    Task<string> MarkAllAsRead();
    Task<string> DisableAll();
}
