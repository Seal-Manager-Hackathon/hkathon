namespace Hackathon.Application.Services.Lecturer.Notification;

public class GetNotificationsRequest
{
    public string? Title { get; set; }
    public string? TargetType { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}