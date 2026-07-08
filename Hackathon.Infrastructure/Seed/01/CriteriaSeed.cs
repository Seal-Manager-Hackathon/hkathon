using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class CriteriaSeed
{
    public static void SeedCriteria(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CriteriaTemplates>().HasData(
            new CriteriaTemplates
            {
                Id = SeedConstants.IdeaCriteriaTemplateId,
                RoundId = SeedConstants.IdeaRoundId,
                Title = "Idea Evaluation Template",
                Description = "Criteria for idea validation",
                IsDisable = false,
                IsActive = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new CriteriaTemplates
            {
                Id = SeedConstants.FinalCriteriaTemplateId,
                RoundId = SeedConstants.FinalRoundId,
                Title = "Final Demo Evaluation Template",
                Description = "Criteria for final demo",
                IsDisable = false,
                IsActive = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New CriteriaTemplates connected to 10 new Rounds
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000010"), Guid.Parse("21000000-0000-0000-0000-000000000010"), "Startup Pitch Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000011"), Guid.Parse("21000000-0000-0000-0000-000000000011"), "Green Tech Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000012"), Guid.Parse("21000000-0000-0000-0000-000000000012"), "SecOps Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000013"), Guid.Parse("21000000-0000-0000-0000-000000000013"), "AI Evaluation Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000014"), Guid.Parse("21000000-0000-0000-0000-000000000014"), "Web3 Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000015"), Guid.Parse("21000000-0000-0000-0000-000000000015"), "Algorithm Performance Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000016"), Guid.Parse("21000000-0000-0000-0000-000000000016"), "Cloud Scaling Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000017"), Guid.Parse("21000000-0000-0000-0000-000000000017"), "Data Science Evaluation Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000018"), Guid.Parse("21000000-0000-0000-0000-000000000018"), "Smart Grids Template"),
            CreateTemplate(Guid.Parse("22000000-0000-0000-0000-000000000019"), Guid.Parse("21000000-0000-0000-0000-000000000019"), "Robotics Control Template")
        );

        modelBuilder.Entity<CriteriaItems>().HasData(
            new CriteriaItems
            {
                Id = SeedConstants.InnovationCriteriaItemId,
                CriteriaTemplateId = SeedConstants.IdeaCriteriaTemplateId,
                Name = "Innovation",
                Description = "Novelty of the idea",
                Score = 40m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new CriteriaItems
            {
                Id = SeedConstants.FeasibilityCriteriaItemId,
                CriteriaTemplateId = SeedConstants.IdeaCriteriaTemplateId,
                Name = "Feasibility",
                Description = "Feasibility of execution",
                Score = 60m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new CriteriaItems
            {
                Id = SeedConstants.TechnicalCriteriaItemId,
                CriteriaTemplateId = SeedConstants.FinalCriteriaTemplateId,
                Name = "Technical Execution",
                Description = "Quality of technical implementation",
                Score = 50m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },
            new CriteriaItems
            {
                Id = SeedConstants.PresentationCriteriaItemId,
                CriteriaTemplateId = SeedConstants.FinalCriteriaTemplateId,
                Name = "Presentation",
                Description = "Clarity of presentation",
                Score = 50m,
                IsDisable = false,
                CreatedAt = SeedConstants.CreatedAt,
                UpdatedAt = SeedConstants.CreatedAt
            },

            // 10 New CriteriaItems connected to 10 new CriteriaTemplates
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000010"), Guid.Parse("22000000-0000-0000-0000-000000000010"), "Business Value", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000011"), Guid.Parse("22000000-0000-0000-0000-000000000011"), "Sustainability Impact", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000012"), Guid.Parse("22000000-0000-0000-0000-000000000012"), "Threat Modelling", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000013"), Guid.Parse("22000000-0000-0000-0000-000000000013"), "Model Performance", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000014"), Guid.Parse("22000000-0000-0000-0000-000000000014"), "Web3 Security Audit", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000015"), Guid.Parse("22000000-0000-0000-0000-000000000015"), "Code Optimization", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000016"), Guid.Parse("22000000-0000-0000-0000-000000000016"), "Deployment Latency", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000017"), Guid.Parse("22000000-0000-0000-0000-000000000017"), "Accuracy Metric", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000018"), Guid.Parse("22000000-0000-0000-0000-000000000018"), "Grid Latency", 100m),
            CreateItem(Guid.Parse("23000000-0000-0000-0000-000000000019"), Guid.Parse("22000000-0000-0000-0000-000000000019"), "Control Loop Latency", 100m)
        );
    }

    private static CriteriaTemplates CreateTemplate(Guid id, Guid roundId, string title)
    {
        return new CriteriaTemplates
        {
            Id = id,
            RoundId = roundId,
            Title = title,
            Description = $"Evaluation template details for {title}",
            IsDisable = false,
            IsActive = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static CriteriaItems CreateItem(Guid id, Guid templateId, string name, decimal maxScore)
    {
        return new CriteriaItems
        {
            Id = id,
            CriteriaTemplateId = templateId,
            Name = name,
            Description = $"Criteria item for {name}",
            Score = maxScore,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
