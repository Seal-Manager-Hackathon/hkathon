using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Tracks : BaseEntity<Guid>, IAuditableEntity
{
    public Guid EventId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }

    public Events Event { get; set; } = null!;
    public ICollection<Topics> Topics { get; set; } = new List<Topics>();
    public ICollection<AssignTracks> AssignTracks { get; set; } = new List<AssignTracks>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}