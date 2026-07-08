namespace Hackathon.Application.Services.Base.Notification;

public class GetNotificationDetailResponse
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TeamId { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
    public string? TargetType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
