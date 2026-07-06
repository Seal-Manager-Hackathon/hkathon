using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Rounds : BaseEntity<Guid>, IAuditableEntity
{
    public Guid EventId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }

    public Events Event { get; set; } = null!;
    public ICollection<CriteriaTemplates> CriteriaTemplates { get; set; } = new List<CriteriaTemplates>();
    public ICollection<RoundDetails> RoundDetails { get; set; } = new List<RoundDetails>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
