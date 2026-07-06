using Hackathon.Repository.Enum;

namespace Hackathon.Service.Notifications;

public static class Response
{
    public class NotificationItemResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public NotificationStatusEnum? Status { get; set; }
        public NotificationTargetTypeEnum TargetType { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
