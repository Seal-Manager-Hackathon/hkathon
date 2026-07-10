using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Mentor.MentorNotification;

public class SendTrackNotificationRequest
{
    [Required(ErrorMessage = "Title Is Required")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description Is Required")]
    public string Description { get; set; } = null!;
}

public class UpdateMentorNotificationRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}
