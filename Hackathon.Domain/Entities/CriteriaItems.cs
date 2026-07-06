using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class CriteriaItems : BaseEntity<Guid>, IAuditableEntity
{
    public Guid CriteriaTemplateId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Score { get; set; }

    public CriteriaTemplates CriteriaTemplate { get; set; } = null!;
    public ICollection<ScoreItems> ScoreItems { get; set; } = new List<ScoreItems>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}