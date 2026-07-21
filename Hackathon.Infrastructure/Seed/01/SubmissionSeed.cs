using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Submission;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class SubmissionSeed
{
    public static void SeedSubmissions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submissions>().HasData(
            CreateSubmission(SeedConstants.SeedInnovatorsIdeaSubmissionId, SeedConstants.SeedInnovatorsIdeaRoundDetailId, "https://seed.local/submissions/seed-innovators-idea", new DateTimeOffset(2026, 6, 21, 10, 0, 0, TimeSpan.Zero), SubmissionStatusEnum.Graded),
            CreateSubmission(SeedConstants.SeedInnovatorsFinalSubmissionId, SeedConstants.SeedInnovatorsFinalRoundDetailId, "https://seed.local/submissions/seed-innovators-final", new DateTimeOffset(2026, 6, 22, 10, 0, 0, TimeSpan.Zero), SubmissionStatusEnum.Graded),
            CreateSubmission(SeedConstants.GreenCodersIdeaSubmissionId, SeedConstants.GreenCodersIdeaRoundDetailId, "https://seed.local/submissions/green-coders-idea", new DateTimeOffset(2026, 6, 21, 10, 30, 0, TimeSpan.Zero), SubmissionStatusEnum.Graded),
            CreateSubmission(SeedConstants.GreenCodersFinalSubmissionId, SeedConstants.GreenCodersFinalRoundDetailId, "https://seed.local/submissions/green-coders-final", new DateTimeOffset(2026, 6, 22, 10, 30, 0, TimeSpan.Zero), SubmissionStatusEnum.Graded),

            CreatePagingSubmission("10", "nexus-startups-idea", 2024),
            CreatePagingSubmission("11", "solar-energy-idea", 2024),
            CreatePagingSubmission("12", "cyber-defenders-idea", 2025),
            CreatePagingSubmission("13", "ai-makers-idea", 2025),
            CreatePagingSubmission("14", "web3-validators-idea", 2025),
            CreatePagingSubmission("15", "seal-elite-idea", 2026),
            CreatePagingSubmission("16", "cloud-deployers-idea", 2026),
            CreatePagingSubmission("17", "data-analytics-idea", 2026),
            CreatePagingSubmission("18", "smart-grids-idea", 2027),
            CreatePagingSubmission("19", "robotics-pathfinder-idea", 2027)
        );
    }

    private static Submissions CreatePagingSubmission(string suffix, string slug, int year)
    {
        return CreateSubmission(
            Guid.Parse($"33000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"32000000-0000-0000-0000-0000000000{suffix}"),
            $"https://seed.local/submissions/{slug}",
            new DateTimeOffset(year, 6, 16, 12, 0, 0, TimeSpan.Zero),
            SubmissionStatusEnum.Graded);
    }

    private static Submissions CreateSubmission(Guid id, Guid roundDetailId, string url, DateTimeOffset submittedAt, SubmissionStatusEnum status)
    {
        return new Submissions
        {
            Id = id,
            RoundDetailId = roundDetailId,
            Url = url,
            Description = "Seed submission",
            Status = status,
            SubmittedAt = submittedAt,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
