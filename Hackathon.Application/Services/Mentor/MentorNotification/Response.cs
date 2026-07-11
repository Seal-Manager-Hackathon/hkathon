namespace Hackathon.Application.Services.Mentor.MentorNotification;

public class MentorTrackItem
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public Guid? EventRoleId { get; set; }
    public string? EventRoleName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetMentorTracksResponse
{
    public List<MentorTrackItem> Tracks { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
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
