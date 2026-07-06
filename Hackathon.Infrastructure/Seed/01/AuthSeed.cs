using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EmailVerification;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class AuthSeed
{
    public static void SeedAuthData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshTokens>().HasData(
            new RefreshTokens
            {
                Id = Guid.Parse("12000000-0000-0000-0000-000000000001"),
                UserId = SeedConstants.AdminUserId,
                RefreshTokenHash = "seed-refresh-token-hash",
                IpAddress = "127.0.0.1",
                UserAgent = "Seed Agent",
                DeviceLabel = "Seed Device",
                ExpiredAt = SeedConstants.CreatedAt.AddDays(7),
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New RefreshTokens
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018")),
            CreateRefreshToken(Guid.Parse("12000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"))
        );

        modelBuilder.Entity<ResetPasswords>().HasData(
            new ResetPasswords
            {
                Id = Guid.Parse("13000000-0000-0000-0000-000000000001"),
                UserId = SeedConstants.StudentMemberUserId,
                TokenHash = "seed-reset-password-token-hash",
                IsUsed = false,
                ExpiresAt = SeedConstants.CreatedAt.AddHours(1),
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New ResetPasswords
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018")),
            CreateResetPassword(Guid.Parse("13000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"))
        );

        modelBuilder.Entity<EmailVerifications>().HasData(
            new EmailVerifications
            {
                Id = Guid.Parse("14000000-0000-0000-0000-000000000001"),
                UserId = SeedConstants.StudentLeaderUserId,
                TokenHash = "seed-email-verification-token-hash",
                ExpiredAt = SeedConstants.CreatedAt.AddDays(1),
                Status = EmailVerificationStatusEnum.Verified,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New EmailVerifications
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000010"), Guid.Parse("10000000-0000-0000-0000-000000000010")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000011"), Guid.Parse("10000000-0000-0000-0000-000000000011")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000012"), Guid.Parse("10000000-0000-0000-0000-000000000012")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000013"), Guid.Parse("10000000-0000-0000-0000-000000000013")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000014"), Guid.Parse("10000000-0000-0000-0000-000000000014")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000015"), Guid.Parse("10000000-0000-0000-0000-000000000015")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000016"), Guid.Parse("10000000-0000-0000-0000-000000000016")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000017"), Guid.Parse("10000000-0000-0000-0000-000000000017")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000018"), Guid.Parse("10000000-0000-0000-0000-000000000018")),
            CreateEmailVerification(Guid.Parse("14000000-0000-0000-0000-000000000019"), Guid.Parse("10000000-0000-0000-0000-000000000019"))
        );
    }

    private static RefreshTokens CreateRefreshToken(Guid id, Guid userId)
    {
        return new RefreshTokens
        {
            Id = id,
            UserId = userId,
            RefreshTokenHash = $"token-{id}",
            IpAddress = "127.0.0.1",
            UserAgent = "Seed Agent",
            DeviceLabel = "Seed Device",
            ExpiredAt = SeedConstants.CreatedAt.AddDays(7),
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static ResetPasswords CreateResetPassword(Guid id, Guid userId)
    {
        return new ResetPasswords
        {
            Id = id,
            UserId = userId,
            TokenHash = $"reset-{id}",
            IsUsed = false,
            ExpiresAt = SeedConstants.CreatedAt.AddHours(1),
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static EmailVerifications CreateEmailVerification(Guid id, Guid userId)
    {
        return new EmailVerifications
        {
            Id = id,
            UserId = userId,
            TokenHash = $"verify-{id}",
            ExpiredAt = SeedConstants.CreatedAt.AddDays(1),
            Status = EmailVerificationStatusEnum.Verified,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
