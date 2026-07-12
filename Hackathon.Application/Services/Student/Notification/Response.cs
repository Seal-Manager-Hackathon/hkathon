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
