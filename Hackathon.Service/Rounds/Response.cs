namespace Hackathon.Service.Rounds;

public static class Response
{
    public class RoundResponse
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? RoundNo { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartSubmission { get; set; }
        public DateTimeOffset? EndSubmission { get; set; }
        public int? LimitTeam { get; set; }
        public bool IsDisable { get; set; }
        public bool IsEnded { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateSubmissionResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid TeamId { get; set; }
        public string? Url { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
    }

    public class RoundDetailResponse
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? RoundNo { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartSubmission { get; set; }
        public DateTimeOffset? EndSubmission { get; set; }
        public int? LimitTeam { get; set; }
        public bool IsDisable { get; set; }
        public bool IsEnded { get; set; }
    }

    public class MyRoundResponse
    {
        public Guid RoundId { get; set; }
        public Guid EventId { get; set; }
        public string RoundName { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public int? RoundNo { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid RegisterTeamId { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartSubmission { get; set; }
        public DateTimeOffset? EndSubmission { get; set; }
        public bool IsEnded { get; set; }
    }

    public class MyRoundDetailResponse
    {
        public Guid RoundId { get; set; }
        public Guid EventId { get; set; }
        public string RoundName { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public int? RoundNo { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid RegisterTeamId { get; set; }
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public DateTimeOffset? StartSubmission { get; set; }
        public DateTimeOffset? EndSubmission { get; set; }
    }

    public class SubmissionResponse
    {
        public Guid SubmissionId { get; set; }
        public string? Url { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public decimal? TotalScore { get; set; }
    }

    public class MyRoundSubmissionResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public Guid RoundDetailId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public bool IsLatest { get; set; }
        public string GradingStatus { get; set; } = null!;
    }

    public class RoundRankingResponse
    {
        public int Rank { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid SubmissionId { get; set; }
        public decimal AverageScore { get; set; }
    }

    public class MyRoundScoreResponse
    {
        public Guid RoundId { get; set; }
        public string? RoundName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid SubmissionId { get; set; }
        public string GradingStatus { get; set; } = null!;
        public string? Message { get; set; }
        public decimal? AverageTotalScore { get; set; }
        public bool IsAppealable { get; set; }
        public List<MyRoundCriteriaScoreResponse> CriteriaScores { get; set; } = new();
    }

    public class TeamLatestSubmissionScoreResponse
    {
        public Guid RoundId { get; set; }
        public string? RoundName { get; set; }
        public Guid TeamId { get; set; }
        public string? TeamName { get; set; }
        public Guid? SubmissionId { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public bool HasSubmission { get; set; }
        public bool IsGraded { get; set; }
        public string GradingStatus { get; set; } = null!;
        public string? Message { get; set; }
        public decimal? AverageTotalScore { get; set; }
        public List<MyRoundCriteriaScoreResponse> CriteriaScores { get; set; } = new();
    }

    public class MyRoundCriteriaScoreResponse
    {
        public Guid CriteriaItemId { get; set; }
        public string CriteriaItemName { get; set; } = null!;
        public decimal? AverageCriteriaScore { get; set; }
        public decimal MaxScore { get; set; }
    }

    public class StaffRoundSubmissionResponse
    {
        public Guid? SubmissionId { get; set; }
        public Guid RoundDetailId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? SubmissionStatus { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string? GradingStatus { get; set; }
        public List<AssignedJudgeResponse> AssignedJudges { get; set; } = new();
        public decimal? AverageScore { get; set; }
        public decimal? MinScore { get; set; }
        public decimal? MaxScore { get; set; }
    }

    public class AssignedJudgeResponse
    {
        public Guid JudgeId { get; set; }
        public string JudgeName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool HasScored { get; set; }
        public decimal? TotalScore { get; set; }
        public bool IsFinalized { get; set; }
    }

    public class AssignJudgesToSubmissionResponse
    {
        public Guid SubmissionId { get; set; }
        public List<AssignedJudgeResponse> AssignedJudges { get; set; } = new();
    }

    public class EndRoundResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public Guid EventId { get; set; }
        public Guid? NextRoundId { get; set; }
        public string? NextRoundName { get; set; }
        public int? NextRoundLimitTeam { get; set; }
        public int TotalTeams { get; set; }
        public int TotalAdvanced { get; set; }
        public string? Message { get; set; }
        public List<AdvancedTeamResponse> Teams { get; set; } = new();
    }

    public class AdvancedTeamResponse
    {
        public int Rank { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public decimal AverageScore { get; set; }
        public Guid LatestSubmissionId { get; set; }
        public bool IsAdvanced { get; set; }
    }
}
