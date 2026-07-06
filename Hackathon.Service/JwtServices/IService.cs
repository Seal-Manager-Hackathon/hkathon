using System.Security.Claims;

namespace Hackathon.Service.JwtServices;

public interface IService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
    public string GenerateEmailVerificationToken(IEnumerable<Claim> claims, double expiration);
    ClaimsPrincipal ValidateToken(string token);
}