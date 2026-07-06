using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.Report;

namespace Hackathon.Domain.Entities;

public class Reports : BaseEntity<Guid>, IAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid? AssignEventId { get; set; }
    public Guid? SubmissionId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImgUrl { get; set; }
    public string? FileUrl { get; set; }
    public ReportStatusEnum? Status { get; set; }
    public string? Reason { get; set; }
    public string? TypeReport { get; set; }
 
    public Users User { get; set; } = null!;
    public AssignEvents? AssignEvent { get; set; }
    public Submissions? Submission { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}