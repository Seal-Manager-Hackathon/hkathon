using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Seed;

public static class ScoreSeed
{
    public static void SeedScores(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Scores>().HasData(
            CreateScore(SeedConstants.SeedInnovatorsIdeaScoreId, SeedConstants.SeedInnovatorsIdeaSubmissionId, SeedConstants.JudgeAiAssignTrackId, 85m),
            CreateScore(SeedConstants.SeedInnovatorsFinalScoreId, SeedConstants.SeedInnovatorsFinalSubmissionId, SeedConstants.JudgeAiAssignTrackId, 90m),
            CreateScore(SeedConstants.GreenCodersIdeaScoreId, SeedConstants.GreenCodersIdeaSubmissionId, SeedConstants.JudgeGreenAssignTrackId, 78m),
            CreateScore(SeedConstants.GreenCodersFinalScoreId, SeedConstants.GreenCodersFinalSubmissionId, SeedConstants.JudgeGreenAssignTrackId, 82m),

            CreatePagingScore("10", 85m),
            CreatePagingScore("11", 89m),
            CreatePagingScore("12", 91m),
            CreatePagingScore("13", 78m),
            CreatePagingScore("14", 88m),
            CreatePagingScore("15", 92m),
            CreatePagingScore("16", 87m),
            CreatePagingScore("17", 83m),
            CreatePagingScore("18", 95m),
            CreatePagingScore("19", 94m)
        );

        modelBuilder.Entity<ScoreItems>().HasData(
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000001"), SeedConstants.SeedInnovatorsIdeaScoreId, SeedConstants.InnovationCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 35m, "Strong innovation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000002"), SeedConstants.SeedInnovatorsIdeaScoreId, SeedConstants.FeasibilityCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 50m, "Feasible plan"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000003"), SeedConstants.SeedInnovatorsFinalScoreId, SeedConstants.TechnicalCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 45m, "Solid implementation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000004"), SeedConstants.SeedInnovatorsFinalScoreId, SeedConstants.PresentationCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 45m, "Clear presentation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000005"), SeedConstants.GreenCodersIdeaScoreId, SeedConstants.InnovationCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 32m, "Useful concept"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000006"), SeedConstants.GreenCodersIdeaScoreId, SeedConstants.FeasibilityCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 46m, "Good execution path"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000007"), SeedConstants.GreenCodersFinalScoreId, SeedConstants.TechnicalCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 40m, "Working prototype"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000008"), SeedConstants.GreenCodersFinalScoreId, SeedConstants.PresentationCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 42m, "Good demo"),

            CreatePagingScoreItem("10", 85m),
            CreatePagingScoreItem("11", 89m),
            CreatePagingScoreItem("12", 91m),
            CreatePagingScoreItem("13", 78m),
            CreatePagingScoreItem("14", 88m),
            CreatePagingScoreItem("15", 92m),
            CreatePagingScoreItem("16", 87m),
            CreatePagingScoreItem("17", 83m),
            CreatePagingScoreItem("18", 95m),
            CreatePagingScoreItem("19", 94m)
        );
    }

    private static Scores CreatePagingScore(string suffix, decimal totalScore)
    {
        return CreateScore(
            Guid.Parse($"50000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"33000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"41000000-0000-0000-0000-0000000000{int.Parse(suffix) + 30}"),
            totalScore);
    }

    private static ScoreItems CreatePagingScoreItem(string suffix, decimal score)
    {
        return CreateScoreItem(
            Guid.Parse($"51000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"50000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"23000000-0000-0000-0000-0000000000{suffix}"),
            Guid.Parse($"41000000-0000-0000-0000-0000000000{int.Parse(suffix) + 30}"),
            score,
            "Paging event evaluation");
    }

    private static Scores CreateScore(Guid id, Guid submissionId, Guid assignTrackId, decimal totalScore)
    {
        return new Scores
        {
            Id = id,
            SubmissionId = submissionId,
            AssignTrackId = assignTrackId,
            IsRetake = false,
            TotalScore = totalScore,
            IsMock = false,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }

    private static ScoreItems CreateScoreItem(Guid id, Guid scoreId, Guid criteriaItemId, Guid assignTrackId, decimal score, string comment)
    {
        return new ScoreItems
        {
            Id = id,
            ScoreId = scoreId,
            CriteriaItemId = criteriaItemId,
            AssignTrackId = assignTrackId,
            Score = score,
            Comment = comment,
            IsDisable = false,
            CreatedAt = SeedConstants.CreatedAt,
            UpdatedAt = SeedConstants.CreatedAt
        };
    }
}
