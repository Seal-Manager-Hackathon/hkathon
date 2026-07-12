namespace Hackathon.Application.Services.Student.User;

public class StudentUserDetailResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string? StudentId { get; set; }
    public string? College { get; set; }
    public string? ImgUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public bool? IsVerified { get; set; }
    public bool IsDisable { get; set; }
    public string? BanReason { get; set; }
    public DateTimeOffset? BannedAt { get; set; }
    public DateTimeOffset? VerifyEmailAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
