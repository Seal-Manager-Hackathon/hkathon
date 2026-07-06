using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.EmailVerification;

namespace Hackathon.Domain.Entities;

public class EmailVerifications : BaseEntity<Guid>, IAuditableEntity
{
    public Guid UserId { get; set; }
    public required string TokenHash { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public EmailVerificationStatusEnum? Status { get; set; }

    public Users User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}