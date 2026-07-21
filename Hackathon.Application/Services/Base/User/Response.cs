using Microsoft.AspNetCore.Http;

namespace Hackathon.Application.Services.Base.User;

public class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }

    public string? StudentId { get; set; }
    public string? ImgUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? AvatarUrl { get; set; }
    public IFormFile? AvatarFile { get; set; }
}

public class GetMyProfileResponse
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
    public string? BanReason { get; set; }
    public DateTimeOffset? BannedAt { get; set; }
    public bool? IsVerified { get; set; }
    public DateTimeOffset? VerifyEmailAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
