namespace Hackathon.Application.Services.Mentor.MentorNotification;

public class MentorTrackItem
{
    public Guid AssignTrackId { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
}

public class SendNotificationResponse
{
    public Guid NotificationId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignTrackId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class GetNotificationsByTrackResponse
{
    public List<MentorNotificationItem> Notifications { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class MentorNotificationItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid AssignTrackId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class MentorNotificationDetailResponse
{
    public Guid Id { get; set; }
    public Guid AssignTrackId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
