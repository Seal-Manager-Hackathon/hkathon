using Hackathon.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Hackathon.Infrastructure.Services.Password;

public class Service : IPasswordService
{
    private readonly SecurityOptions _options;

    public Service(IOptions<SecurityOptions> options)
    {
        _options = options.Value;
    }

    public string HashPassword(string rawPassword)
    {
        var peppered = rawPassword + _options.Pepper;
        return BCrypt.Net.BCrypt.EnhancedHashPassword(peppered, hashType: BCrypt.Net.HashType.SHA256);
    }

    public bool VerifyPassword(string rawPassword, string hashedPassword)
    {
        var peppered = rawPassword + _options.Pepper;
        return BCrypt.Net.BCrypt.EnhancedVerify(peppered, hashedPassword, hashType: BCrypt.Net.HashType.SHA256);
    }
}
