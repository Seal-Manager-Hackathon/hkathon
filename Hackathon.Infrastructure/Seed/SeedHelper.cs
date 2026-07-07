using Hackathon.Infrastructure.Services.Password;

namespace Hackathon.Infrastructure.Seed;

public static class SeedHelper
{
    // Phải khớp với SecurityOptions.Pepper trong appsettings.json
    private const string SeedPepper = "Matkhaubimat123";

    private static string? _cachedHash;

    /// <summary>
    /// BCrypt EnhancedHash (SHA256) của password "string" + Pepper,
    /// dùng chung cho tất cả seed user — giống cơ chế hash của IPasswordService khi login/register.
    /// Hash được cache sau lần gọi đầu tiên.
    /// </summary>
    public static string HashDefaultPassword()
    {
        if (_cachedHash == null)
        {
            _cachedHash = BCrypt.Net.BCrypt.EnhancedHashPassword("string" + SeedPepper, hashType: BCrypt.Net.HashType.SHA256);
        }
        return _cachedHash;
    }
}
