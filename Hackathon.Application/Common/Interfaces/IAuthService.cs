using Hackathon.Application.Services.Auth;

namespace Hackathon.Application.Common.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest request);
    Task<VerifyEmailResponse> VerifyEmail(VerifyEmailRequest request);
    Task<LoginResponse> Login(LoginRequest request);
}
