using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.Invitation;

namespace Hackathon.Domain.Entities;

public class Invitations : BaseEntity<Guid>, IAuditableEntity
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset? LimitTime { get; set; }
    public InvitationStatusEnum? Status { get; set; }
    public string? Description { get; set; }

    public Teams Team { get; set; } = null!;
    public Users User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}