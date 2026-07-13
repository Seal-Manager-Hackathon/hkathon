using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.Submission;

public class CreateSubmissionRequest
{
    [Required(ErrorMessage = "RegisterTeamId Is Required")]
    public Guid RegisterTeamId { get; set; }

    [Required(ErrorMessage = "RoundId Is Required")]
    public Guid RoundId { get; set; }

    [Required(ErrorMessage = "Url Is Required")]
    public string Url { get; set; } = null!;

    public string? Description { get; set; }
}
