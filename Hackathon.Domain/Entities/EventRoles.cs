using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.EventRole;

namespace Hackathon.Domain.Entities;

public class EventRoles : BaseEntity<Guid>, IAuditableEntity
{
    public EventRoleEnum Name { get; set; }

    public ICollection<AssignEvents> AssignEvents { get; set; } = new List<AssignEvents>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}