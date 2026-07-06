using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Submission;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class SubmissionSeed
{
    public static void SeedSubmissions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submissions>().HasData(
            CreateSubmission(SeedConstants.SeedInnovatorsIdeaSubmissionId, SeedConstants.SeedInnovatorsIdeaRoundDetailId, "https://seed.local/submissions/seed-innovators-idea"),
            CreateSubmission(SeedConstants.SeedInnovatorsFinalSubmissionId, SeedConstants.SeedInnovatorsFinalRoundDetailId, "https://seed.local/submissions/seed-innovators-final"),
            CreateSubmission(SeedConstants.GreenCodersIdeaSubmissionId, SeedConstants.GreenCodersIdeaRoundDetailId, "https://seed.local/submissions/green-coders-idea"),
            CreateSubmission(SeedConstants.GreenCodersFinalSubmissionId, SeedConstants.GreenCodersFinalRoundDetailId, "https://seed.local/submissions/green-coders-final"),

            // 10 New Submissions connected to 10 new RoundDetails
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000010"), Guid.Parse("32000000-0000-0000-0000-000000000010"), "https://seed.local/submissions/nexus-startups-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000011"), Guid.Parse("32000000-0000-0000-0000-000000000011"), "https://seed.local/submissions/solar-energy-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000012"), Guid.Parse("32000000-0000-0000-0000-000000000012"), "https://seed.local/submissions/cyber-defenders-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000013"), Guid.Parse("32000000-0000-0000-0000-000000000013"), "https://seed.local/submissions/ai-makers-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000014"), Guid.Parse("32000000-0000-0000-0000-000000000014"), "https://seed.local/submissions/web3-validators-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000015"), Guid.Parse("32000000-0000-0000-0000-000000000015"), "https://seed.local/submissions/seal-elite-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000016"), Guid.Parse("32000000-0000-0000-0000-000000000016"), "https://seed.local/submissions/cloud-deployers-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000017"), Guid.Parse("32000000-0000-0000-0000-000000000017"), "https://seed.local/submissions/data-analytics-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000018"), Guid.Parse("32000000-0000-0000-0000-000000000018"), "https://seed.local/submissions/smart-grids-idea"),
            CreateSubmission(Guid.Parse("33000000-0000-0000-0000-000000000019"), Guid.Parse("32000000-0000-0000-0000-000000000019"), "https://seed.local/submissions/robotics-pathfinder-idea")
        );
    }

    private static Submissions CreateSubmission(Guid id, Guid roundDetailId, string url)
    {
        return new Submissions
        {
            Id = id,
            RoundDetailId = roundDetailId,
            Url = url,
            Description = "Seed submission",
            Status = SubmissionStatusEnum.Submitted,
            SubmittedAt = SeedConstants.CreatedAt.AddDays(10),
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
