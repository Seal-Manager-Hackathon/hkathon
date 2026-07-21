using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class EventSeed
{
    public static void SeedEvents(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Events>().HasData(
            new Events
            {
                Id = SeedConstants.SealHackathonEventId,
                Name = "SEAL Hackathon 2026",
                Description = "Seed event for hackathon demo data",
                StartTime = SeedConstants.CreatedAt.AddDays(10),
                EndTime = SeedConstants.CreatedAt.AddDays(12),
                RegisterLimitTime = SeedConstants.CreatedAt.AddDays(5),
                LimitTeam = 20,
                MinMember = 2,
                MaxMember = 4,
                Status = EventStatusEnum.Closed,
                NumberRound = 2,
                Season = SeasonEnum.Winter,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Seed Events
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000010"), "Tech Startup Challenge 2024", 2024, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000011"), "Green Innovators Cup 2024", 2024, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000012"), "Cyber Security Arena 2025", 2025, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000013"), "AI HackFest 2025", 2025, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000014"), "Web3 DevCon 2025", 2025, EventStatusEnum.Closed, true), // Disabled event
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000015"), "SEAL Coding Battle 2026", 2026, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000016"), "Cloud Builders Cup 2026", 2026, EventStatusEnum.Closed, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000017"), "Data Science Hackathon 2026", 2026, EventStatusEnum.Closed, true), // Disabled event
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000018"), "IoT Smart City 2027", 2027, EventStatusEnum.Published, false),
            CreateEvent(Guid.Parse("20000000-0000-0000-0000-000000000019"), "Robotics Summit 2027", 2027, EventStatusEnum.Published, false)
        );
    }

    private static Events CreateEvent(Guid id, string name, int startYear, EventStatusEnum status, bool isDisable)
    {
        var startTime = new DateTimeOffset(startYear, 6, 15, 9, 0, 0, TimeSpan.Zero);
        return new Events
        {
            Id = id,
            Name = name,
            Description = $"Detailed description for {name}",
            StartTime = startTime,
            EndTime = startTime.AddDays(3),
            RegisterLimitTime = startTime.AddDays(-5),
            LimitTeam = 30,
            MinMember = 2,
            MaxMember = 5,
            Status = status,
            NumberRound = 1,
            Season = SeasonEnum.Winter,
            IsDisable = isDisable,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
