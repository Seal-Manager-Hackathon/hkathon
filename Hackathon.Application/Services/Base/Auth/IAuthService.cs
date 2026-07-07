namespace Hackathon.Application.Services.Base.Auth;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest request);
    Task<VerifyEmailResponse> VerifyEmail(VerifyEmailRequest request);
    Task<LoginResponse> Login(LoginRequest request);
    Task<CurrentUserResponse> GetCurrentUser();
    Task ForgotPassword(ForgotPasswordRequest request);
    Task ResetPassword(ResetPasswordRequest request);
}
