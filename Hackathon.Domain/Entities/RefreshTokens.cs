using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class RefreshTokens : BaseEntity<Guid>, IAuditableEntity
{
    public Guid UserId { get; set; }
    public required string RefreshTokenHash { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceLabel { get; set; }
    public DateTimeOffset ExpiredAt { get; set; }
    public DateTimeOffset? RevokedAt { get; set; }

    public Users User { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}