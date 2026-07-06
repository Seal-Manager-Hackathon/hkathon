namespace Hackathon.Service.Auths;

public interface IService
{
    public Task<string> Register(Request.RegisterRequest request);

    public Task<Response.AuthResponse> RefreshToken();
    public Task<Response.VerifyEmailResponse?> VerifyEmail(Request.VerifyEmailRequest request);
    public Task<Response.GetMeResponse> GetMe();
    public Task<string> Logout();
    
    public Task<Response.LoginResponse> LoginAsync(
        Request.LoginRequest request
    );
    public Task<string> ChangePassword(Request.ChangePasswordRequest request);
    public Task<string> ForgotPassword(Request.ForgotPasswordRequest request);
    public Task<string> ResetPassword(Request.ResetPasswordRequest request);
    public Task<string> ResendEmailVerification(Request.ResendEmailVerificationRequest request);
}