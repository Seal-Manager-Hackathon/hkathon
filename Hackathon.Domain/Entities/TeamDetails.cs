using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.TeamDetail;

namespace Hackathon.Domain.Entities;

public class TeamDetails : BaseEntity<Guid>, IAuditableEntity
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public bool IsLeader { get; set; }
    public TeamDetailStatusEnum? Status { get; set; }

    public Teams Team { get; set; } = null!;
    public Users User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}