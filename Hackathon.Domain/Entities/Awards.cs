using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Awards : BaseEntity<Guid>, IAuditableEntity
{
    public Guid EventId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required int LevelAward { get; set; }
    public int? NumberOfAward { get; set; } = 1;
    public decimal? Prize { get; set; }

    public Events Event { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}