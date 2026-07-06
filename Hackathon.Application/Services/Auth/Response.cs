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
