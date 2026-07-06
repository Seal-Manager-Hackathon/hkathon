using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.Infrastructure.Services.Jwt;

public class Service : IJwtService
{
    private readonly JwtOptions _jwtOption;
    private readonly ILogger<Service> _logger;

    public Service(IOptions<JwtOptions> jwtOption, ILogger<Service> logger)
    {
        _jwtOption = jwtOption.Value;
        _logger = logger;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        return GenerateToken(claims, _jwtOption.ExpireMinutes);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public string GenerateEmailVerificationToken(IEnumerable<Claim> claims, double expiration)
    {
        return GenerateToken(claims, expiration);
    }

    private string GenerateToken(IEnumerable<Claim> claims, double expireMinutes)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));

        var signingCredentials = new SigningCredentials(
            secretKey,
            SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtOption.SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _jwtOption.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOption.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(ex, "Token Validation Failed");
            throw new UnauthorizedException(ErrorMessage.Auth.TokenValidationFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Error During Token Validation");
            throw new UnauthorizedException(ErrorMessage.Auth.InvalidOrExpiredToken);
        }
    }
}
