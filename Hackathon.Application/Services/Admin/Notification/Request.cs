using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Admin.Notification;

public class DeleteNotificationRequest
{
    public Guid NotificationId { get; set; }
}

public class UpdateNotificationRequest
{
    public Guid NotificationId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class CreateNotificationRequest
{
    [Required(ErrorMessage = "Title Is Required")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description Is Required")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "TargetType Is Required")]
    public string TargetType { get; set; } = null!;

    public Guid? UserId { get; set; }
    public Guid? TeamId { get; set; }
}



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
