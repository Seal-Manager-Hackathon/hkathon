using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class ResetPasswords : BaseEntity<Guid>, IAuditableEntity
{
    public Guid UserId { get; set; }
    public required string TokenHash { get; set; }
    public bool IsUsed { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }

    public Users User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}