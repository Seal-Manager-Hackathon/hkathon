using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.Notification;

namespace Hackathon.Domain.Entities;

public class Notifications : BaseEntity<Guid>, IAuditableEntity
{
    public Guid? UserId { get; set; }
    public Guid? TeamId { get; set; }
    public string? Title { get; set; }
    public NotificationStatusEnum? Status { get; set; }
    public string? Description { get; set; }
    public NotificationTargetTypeEnum TargetType { get; set; } = NotificationTargetTypeEnum.Personal;

    public Users? User { get; set; }
    public Teams? Team { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}