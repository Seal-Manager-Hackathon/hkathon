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
            CreateScore(SeedConstants.GreenCodersFinalScoreId, SeedConstants.GreenCodersFinalSubmissionId, SeedConstants.JudgeGreenAssignTrackId, 82m)
        );

        modelBuilder.Entity<ScoreItems>().HasData(
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000001"), SeedConstants.SeedInnovatorsIdeaScoreId, SeedConstants.InnovationCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 35m, "Strong innovation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000002"), SeedConstants.SeedInnovatorsIdeaScoreId, SeedConstants.FeasibilityCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 50m, "Feasible plan"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000003"), SeedConstants.SeedInnovatorsFinalScoreId, SeedConstants.TechnicalCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 45m, "Solid implementation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000004"), SeedConstants.SeedInnovatorsFinalScoreId, SeedConstants.PresentationCriteriaItemId, SeedConstants.JudgeAiAssignTrackId, 45m, "Clear presentation"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000005"), SeedConstants.GreenCodersIdeaScoreId, SeedConstants.InnovationCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 32m, "Useful concept"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000006"), SeedConstants.GreenCodersIdeaScoreId, SeedConstants.FeasibilityCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 46m, "Good execution path"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000007"), SeedConstants.GreenCodersFinalScoreId, SeedConstants.TechnicalCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 40m, "Working prototype"),
            CreateScoreItem(Guid.Parse("51000000-0000-0000-0000-000000000008"), SeedConstants.GreenCodersFinalScoreId, SeedConstants.PresentationCriteriaItemId, SeedConstants.JudgeGreenAssignTrackId, 42m, "Good demo")
        );
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
