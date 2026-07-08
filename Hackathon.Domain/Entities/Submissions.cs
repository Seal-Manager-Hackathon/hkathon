using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.Submission;

namespace Hackathon.Domain.Entities;

public class Submissions : BaseEntity<Guid>, IAuditableEntity
{
    public Guid RoundDetailId { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public SubmissionStatusEnum? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public bool IsRegrade { get; set; }

    public RoundDetails RoundDetail { get; set; } = null!;
    public ICollection<Scores> Scores { get; set; } = new List<Scores>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}