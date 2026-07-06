using System.Security.Claims;

namespace Hackathon.Application.Common.Interfaces;

public interface IJwtService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    public string GenerateRefreshToken();
    public string GenerateEmailVerificationToken(IEnumerable<Claim> claims, double expiration);
    ClaimsPrincipal ValidateToken(string token);
}