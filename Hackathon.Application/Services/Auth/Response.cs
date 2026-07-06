namespace Hackathon.Application.Services.Auth;

public class AuthResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

public class RegisterResponse
{
    public string Message { get; set; } = null!;
}

public class VerifyEmailResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

public class LoginResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}

public class CurrentUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Address { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string College { get; set; } = string.Empty;
    public string? ImgUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string Role { get; set; } = null!;
    public DateTimeOffset? VerifyEmailAt { get; set; }
    public string? Status { get; set; }
    public string? BanReason { get; set; }
    public DateTimeOffset? BannedAt { get; set; }
    public bool? IsVerified { get; set; }
}
