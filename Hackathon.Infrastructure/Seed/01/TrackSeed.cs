using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class TrackSeed
{
    public static void SeedTracks(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tracks>().HasData(
            new Tracks
            {
                Id = SeedConstants.AiTrackId,
                EventId = SeedConstants.SealHackathonEventId,
                Title = "AI for Education",
                Description = "AI solutions for learning",
                MaxTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Tracks
            {
                Id = SeedConstants.GreenTrackId,
                EventId = SeedConstants.SealHackathonEventId,
                Title = "Green Technology",
                Description = "Sustainable technology solutions",
                MaxTeam = 10,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Seed Tracks
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000010"), Guid.Parse("20000000-0000-0000-0000-000000000010"), "E-Commerce Innovation", "Tech Startup Challenge 2024 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000011"), Guid.Parse("20000000-0000-0000-0000-000000000011"), "Renewable Energy Solutions", "Green Innovators Cup 2024 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000012"), Guid.Parse("20000000-0000-0000-0000-000000000012"), "Intrusion Detection Systems", "Cyber Security Arena 2025 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000013"), Guid.Parse("20000000-0000-0000-0000-000000000013"), "Natural Language Generation", "AI HackFest 2025 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000014"), Guid.Parse("20000000-0000-0000-0000-000000000014"), "Smart Contract Audit", "Web3 DevCon 2025 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000015"), Guid.Parse("20000000-0000-0000-0000-000000000015"), "Algorithm Optimization", "SEAL Coding Battle 2026 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000016"), Guid.Parse("20000000-0000-0000-0000-000000000016"), "Serverless Deployment", "Cloud Builders Cup 2026 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000017"), Guid.Parse("20000000-0000-0000-0000-000000000017"), "Predictive Analytics", "Data Science Hackathon 2026 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000018"), Guid.Parse("20000000-0000-0000-0000-000000000018"), "Smart Grids Infrastructure", "IoT Smart City 2027 Track"),
            CreateTrack(Guid.Parse("24000000-0000-0000-0000-000000000019"), Guid.Parse("20000000-0000-0000-0000-000000000019"), "Robotic Pathfinding", "Robotics Summit 2027 Track")
        );

        modelBuilder.Entity<Topics>().HasData(
            new Topics
            {
                Id = SeedConstants.AiTopicId,
                TrackId = SeedConstants.AiTrackId,
                Title = "Personalized Learning Assistant",
                Description = "Learning assistant powered by AI",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new Topics
            {
                Id = SeedConstants.GreenTopicId,
                TrackId = SeedConstants.GreenTrackId,
                Title = "Carbon Footprint Tracker",
                Description = "Track and reduce carbon footprint",
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New Seed Topics
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000010"), Guid.Parse("24000000-0000-0000-0000-000000000010"), "Retail AI Chatbot", "Automate customer support"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000011"), Guid.Parse("24000000-0000-0000-0000-000000000011"), "Solar Panel Optimization", "Improve efficiency"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000012"), Guid.Parse("24000000-0000-0000-0000-000000000012"), "Malware Classifier AI", "Network security agent"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000013"), Guid.Parse("24000000-0000-0000-0000-000000000013"), "LLM Code Assistant", "Generate code snippets"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000014"), Guid.Parse("24000000-0000-0000-0000-000000000014"), "DeFi Yield Optimizer", "Smart contract platform"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000015"), Guid.Parse("24000000-0000-0000-0000-000000000015"), "Dynamic Pathfinding", "Optimize route calculations"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000016"), Guid.Parse("24000000-0000-0000-0000-000000000016"), "SaaS Scale Engine", "Automated scaling agent"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000017"), Guid.Parse("24000000-0000-0000-0000-000000000017"), "Churn Predictor Model", "Identify customer churn risks"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000018"), Guid.Parse("24000000-0000-0000-0000-000000000018"), "Grid Monitoring Agent", "Smart sensor nodes"),
            CreateTopic(Guid.Parse("25000000-0000-0000-0000-000000000019"), Guid.Parse("24000000-0000-0000-0000-000000000019"), "LiDAR Mapping Agent", "Autonomous LiDAR paths")
        );
    }

    private static Tracks CreateTrack(Guid id, Guid eventId, string title, string description)
    {
        return new Tracks
        {
            Id = id,
            EventId = eventId,
            Title = title,
            Description = description,
            MaxTeam = 15,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static Topics CreateTopic(Guid id, Guid trackId, string title, string description)
    {
        return new Topics
        {
            Id = id,
            TrackId = trackId,
            Title = title,
            Description = description,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
