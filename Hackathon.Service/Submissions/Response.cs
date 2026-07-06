namespace Hackathon.Service.Submissions;

public static class Response
{
    public class SubmissionDetailResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundDetailId { get; set; }
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = null!;
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Guid RegisterTeamId { get; set; }
        public Guid EventId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string GradingStatus { get; set; } = null!;
        public string? Message { get; set; }
        public SubmissionScoreResponse? Score { get; set; }
    }

    public class SubmissionScoreResponse
    {
        public decimal AverageTotalScore { get; set; }
        public bool IsAppealable { get; set; }
        public List<CriteriaScoreResponse> CriteriaScores { get; set; } = new();
    }

    public class CriteriaScoreResponse
    {
        public Guid CriteriaItemId { get; set; }
        public string CriteriaItemName { get; set; } = null!;
        public decimal? AverageCriteriaScore { get; set; }
        public decimal MaxScore { get; set; }
    }

    public class SubmitRoundProjectResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid TeamId { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class RoundSubmissionItemResponse
    {
        public Guid SubmissionId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Hackathon.Repository.Enum.SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public bool IsLatest { get; set; }
    }
}
