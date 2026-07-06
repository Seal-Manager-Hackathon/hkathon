using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class AwardSeed
{
    public static void SeedAwards(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Awards>().HasData(
            new Awards
            {
                Id = SeedConstants.ChampionAwardId,
                EventId = SeedConstants.SealHackathonEventId,
                Name = "Champion",
                Description = "First place award",
                LevelAward = 1,
                NumberOfAward = 1,
                Prize = 1000m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Awards
            {
                Id = SeedConstants.RunnerUpAwardId,
                EventId = SeedConstants.SealHackathonEventId,
                Name = "Runner Up",
                Description = "Second place award",
                LevelAward = 2,
                NumberOfAward = 1,
                Prize = 500m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Awards for 10 new Events
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000010"), Guid.Parse("20000000-0000-0000-0000-000000000010"), "First Prize", "First place award", 1, 1, 1000m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000011"), Guid.Parse("20000000-0000-0000-0000-000000000011"), "First Prize", "First place award", 1, 1, 1200m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000012"), Guid.Parse("20000000-0000-0000-0000-000000000012"), "First Prize", "First place award", 1, 1, 1500m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000013"), Guid.Parse("20000000-0000-0000-0000-000000000013"), "First Prize", "First place award", 1, 1, 1000m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000014"), Guid.Parse("20000000-0000-0000-0000-000000000014"), "First Prize", "First place award", 1, 1, 1100m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000015"), Guid.Parse("20000000-0000-0000-0000-000000000015"), "First Prize", "First place award", 1, 1, 2000m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000016"), Guid.Parse("20000000-0000-0000-0000-000000000016"), "First Prize", "First place award", 1, 1, 1500m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000017"), Guid.Parse("20000000-0000-0000-0000-000000000017"), "First Prize", "First place award", 1, 1, 1000m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000018"), Guid.Parse("20000000-0000-0000-0000-000000000018"), "First Prize", "First place award", 1, 1, 1300m),
            CreateAward(Guid.Parse("26000000-0000-0000-0000-000000000019"), Guid.Parse("20000000-0000-0000-0000-000000000019"), "First Prize", "First place award", 1, 1, 2500m)
        );
    }

    private static Awards CreateAward(Guid id, Guid eventId, string name, string description, int levelAward, int numberOfAward, decimal prize)
    {
        return new Awards
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Description = description,
            LevelAward = levelAward,
            NumberOfAward = numberOfAward,
            Prize = prize,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
