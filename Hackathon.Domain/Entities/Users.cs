using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Domain.Entities;

public class Users : BaseEntity<Guid>, IAuditableEntity
{
    public required string Email { get; set; }
    public required string HashPassword { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Address { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; } = DateTimeOffset.MinValue;
    public string StudentId { get; set; } = string.Empty;
    public string College { get; set; } = string.Empty;
    public string? ImgUrl { get; set; }
    public string? LinkUrl { get; set; }
    public required RoleEnum Role { get; set; }
    public DateTimeOffset? VerifyEmailAt { get; set; }
    public UserStatusEnum? Status { get; set; }
    public string? BanReason { get; set; }
    public DateTimeOffset? BannedAt { get; set; }
    public bool? IsVerified { get; set; }

    public ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();
    public ICollection<ResetPasswords> ResetPasswords { get; set; } = new List<ResetPasswords>();
    public ICollection<EmailVerifications> EmailVerifications { get; set; } = new List<EmailVerifications>();
    public ICollection<TeamDetails> TeamDetails { get; set; } = new List<TeamDetails>();
    public ICollection<Invitations> Invitations { get; set; } = new List<Invitations>();
    public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
    public ICollection<AssignEvents> AssignEvents { get; set; } = new List<AssignEvents>();
    public ICollection<Reports> Reports { get; set; } = new List<Reports>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}