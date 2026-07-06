using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class MentorNotifications : BaseEntity<Guid>, IAuditableEntity
{
    public Guid AssignTrackId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public AssignTracks AssignTrack { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}