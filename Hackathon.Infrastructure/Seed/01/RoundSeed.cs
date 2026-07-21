using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class RoundSeed
{
    public static void SeedRounds(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rounds>().HasData(
            new Rounds
            {
                Id = SeedConstants.IdeaRoundId,
                EventId = SeedConstants.SealHackathonEventId,
                Name = "Idea Submission",
                Description = "Submit and validate the idea",
                RoundNo = 1,
                StartTime = SeedConstants.CreatedAt.AddDays(10),
                EndTime = SeedConstants.CreatedAt.AddDays(11),
                StartSubmission = SeedConstants.CreatedAt.AddDays(10),
                EndSubmission = SeedConstants.CreatedAt.AddDays(10).AddHours(12),
                LimitTeam = 20,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Rounds
            {
                Id = SeedConstants.FinalRoundId,
                EventId = SeedConstants.SealHackathonEventId,
                Name = "Final Demo",
                Description = "Present the final product",
                RoundNo = 2,
                StartTime = SeedConstants.CreatedAt.AddDays(11),
                EndTime = SeedConstants.CreatedAt.AddDays(12),
                StartSubmission = SeedConstants.CreatedAt.AddDays(11),
                EndSubmission = SeedConstants.CreatedAt.AddDays(11).AddHours(12),
                LimitTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Seed Rounds
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000010"), Guid.Parse("20000000-0000-0000-0000-000000000010"), "Idea Submission 2024", 2024, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000011"), Guid.Parse("20000000-0000-0000-0000-000000000011"), "Concept Selection 2024", 2024, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000012"), Guid.Parse("20000000-0000-0000-0000-000000000012"), "Security Design 2025", 2025, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000013"), Guid.Parse("20000000-0000-0000-0000-000000000013"), "AI Model Pitch 2025", 2025, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000014"), Guid.Parse("20000000-0000-0000-0000-000000000014"), "Web3 Auditing 2025", 2025, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000015"), Guid.Parse("20000000-0000-0000-0000-000000000015"), "Rapid Prototyping 2026", 2026, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000016"), Guid.Parse("20000000-0000-0000-0000-000000000016"), "Architecture Demo 2026", 2026, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000017"), Guid.Parse("20000000-0000-0000-0000-000000000017"), "Model Training Pitch 2026", 2026, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000018"), Guid.Parse("20000000-0000-0000-0000-000000000018"), "Smart Grids Pitch 2027", 2027, 1),
            CreateRound(Guid.Parse("21000000-0000-0000-0000-000000000019"), Guid.Parse("20000000-0000-0000-0000-000000000019"), "Robotics Control Pitch 2027", 2027, 1)
        );
    }

    private static Rounds CreateRound(Guid id, Guid eventId, string name, int startYear, int roundNo)
    {
        var startTime = new DateTimeOffset(startYear, 6, 16, 9, 0, 0, TimeSpan.Zero);
        return new Rounds
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Description = $"Round description for {name}",
            RoundNo = roundNo,
            StartTime = startTime,
            EndTime = startTime.AddDays(1),
            StartSubmission = startTime,
            EndSubmission = startTime.AddHours(12),
            LimitTeam = 25,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
