using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.Invitation;

public class SendInvitationRequest
{
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;
}

public class GetInvitationsRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
