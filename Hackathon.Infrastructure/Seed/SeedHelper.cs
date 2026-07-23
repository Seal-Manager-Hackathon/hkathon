namespace Hackathon.Infrastructure.Seed;

public static class SeedHelper
{
    /// <summary>
    /// Fixed BCrypt EnhancedHash (SHA256) of the password "string" + Pepper
    /// (Pepper = SecurityOptions:Pepper, the value used by the live application).
    ///
    /// Hardcoded on purpose so EF Core HasData seed values stay deterministic across
    /// migration generations. BCrypt uses a random salt, so computing the hash at
    /// design time would yield a different value every run and EF would emit an
    /// UpdateData for every seed user on each new migration. This constant is the
    /// exact hash already frozen in the Initial migration, so existing seed rows
    /// never churn and new seed rows share the same valid hash.
    ///
    /// Runtime login/registration is unaffected: IPasswordService recomputes BCrypt
    /// with the live pepper from configuration and compares against this stored hash.
    /// </summary>
    private const string SeedPasswordHash =
        "$2a$11$FTrHahxSf5lojw6joRVC3.ArTfL/2tspZvqA/5i3FeZH1k.ATyvze";

    public static string HashDefaultPassword() => SeedPasswordHash;
}
