namespace Hackathon.Application.Services.Student.Notification;

public class GetStudentNotificationsResponse
{
    public List<StudentNotificationItem> Notifications { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class StudentNotificationItem
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

public class GetMentorNotificationsResponse
{
    public List<StudentMentorNotificationItem> Notifications { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class StudentMentorNotificationItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? MentorFirstName { get; set; }
    public string? MentorLastName { get; set; }
    public string? MentorEmail { get; set; }
    public string? MentorAvatarUrl { get; set; }
    public string? TrackTitle { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class NotificationCountResponse
{
    public int Total { get; set; }
}

public class GetUnreadCountResponse
{
    public int Count { get; set; }
}

public class StudentNotificationDetailResponse
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

public class StudentMentorNotificationDetailResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? MentorUserId { get; set; }
    public string? MentorFirstName { get; set; }
    public string? MentorLastName { get; set; }
    public string? MentorEmail { get; set; }
    public string? MentorAvatarUrl { get; set; }
    public string? MentorRole { get; set; }
    public Guid? EventId { get; set; }
    public string? EventName { get; set; }
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
