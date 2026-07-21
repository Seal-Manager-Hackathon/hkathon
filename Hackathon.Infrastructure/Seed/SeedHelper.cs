using Microsoft.Extensions.Configuration;

namespace Hackathon.Infrastructure.Seed;

public static class SeedHelper
{
    private static string? _cachedPepper;
    private static string? _cachedHash;

    /// <summary>
    /// BCrypt EnhancedHash (SHA256) of password "string" + Pepper,
    /// shared by all seed users — same hash mechanism as IPasswordService during login/register.
    /// Pepper is read from appsettings.json (SecurityOptions.Pepper).
    /// Hash is cached after the first call.
    /// </summary>
    public static string HashDefaultPassword()
    {
        if (_cachedHash != null) return _cachedHash;
        var pepper = GetSeedPepper();
        _cachedHash = BCrypt.Net.BCrypt.EnhancedHashPassword("string" + pepper, hashType: BCrypt.Net.HashType.SHA256);
        return _cachedHash;
    }

    private static string GetSeedPepper()
    {
        if (_cachedPepper != null) return _cachedPepper;

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        _cachedPepper = config["SecurityOptions:Pepper"]
            ?? throw new InvalidOperationException(
                "Missing SecurityOptions:Pepper in configuration. " +
                "Set it in appsettings.json or environment variable SecurityOptions__Pepper.");

        return _cachedPepper;
    }
}
