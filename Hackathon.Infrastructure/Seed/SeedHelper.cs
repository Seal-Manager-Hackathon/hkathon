using Hackathon.Infrastructure.Services.Password;

namespace Hackathon.Infrastructure.Seed;

public static class SeedHelper
{
    // Must match SecurityOptions.Pepper in appsettings.json
    private const string SeedPepper = "Matkhaubimat123";

    private static string? _cachedHash;

    /// <summary>
    /// BCrypt EnhancedHash (SHA256) of password "string" + Pepper,
    /// shared by all seed users — same hash mechanism as IPasswordService during login/register.
    /// Hash is cached after the first call.
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
