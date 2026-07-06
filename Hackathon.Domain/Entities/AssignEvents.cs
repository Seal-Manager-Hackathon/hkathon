using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class AssignEvents : BaseEntity<Guid>, IAuditableEntity
{
    public Guid UserId { get; set; }
    public Guid? EventRoleId { get; set; }
    public Guid EventId { get; set; }

    public Users User { get; set; } = null!;
    public EventRoles? EventRole { get; set; }
    public Events Event { get; set; } = null!;
    public ICollection<AssignTracks> AssignTracks { get; set; } = new List<AssignTracks>();
    public ICollection<Reports> Reports { get; set; } = new List<Reports>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}