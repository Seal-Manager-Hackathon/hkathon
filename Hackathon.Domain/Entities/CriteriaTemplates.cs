using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class CriteriaTemplates : BaseEntity<Guid>, IAuditableEntity
{
    public Guid RoundId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public Rounds Round { get; set; } = null!;
    public ICollection<CriteriaItems> CriteriaItems { get; set; } = new List<CriteriaItems>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}