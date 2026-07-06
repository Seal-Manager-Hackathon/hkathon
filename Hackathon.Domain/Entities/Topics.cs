using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Topics : BaseEntity<Guid>, IAuditableEntity
{
    public Guid TrackId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }

    public Tracks Track { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}