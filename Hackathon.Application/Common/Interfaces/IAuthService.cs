using Hackathon.Application.Services.Auth;

namespace Hackathon.Application.Common.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<VerifyEmailResponse> VerifyEmailAsync(VerifyEmailRequest request);
}
