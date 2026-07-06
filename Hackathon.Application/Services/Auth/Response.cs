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
    public string? Role { get; set; }
}
