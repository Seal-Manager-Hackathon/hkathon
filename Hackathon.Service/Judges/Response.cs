using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Judges;

public static class Response
{
    public class JudgeTrackResponse
    {
        public Guid AssignTrackId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public string? TrackDescription { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public int SubmissionCount { get; set; }
        public int GradedSubmissionCount { get; set; }
    }

    public class JudgeTrackSubmissionResponse
    {
        public Guid? SubmissionId { get; set; }
        public Guid RoundDetailId { get; set; }
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string? Url { get; set; }
        public string? Description { get; set; }
        public SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string GradingStatus { get; set; } = "Pending";
        public Guid? ScoreId { get; set; }
        public decimal? TotalScore { get; set; }
    }

    public class SubmissionCriteriaResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundId { get; set; }
        public Guid? TemplateId { get; set; }
        public string? TemplateTitle { get; set; }
        public List<CriteriaItemResponse> CriteriaItems { get; set; } = [];
    }

    public class CriteriaItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal MaxScore { get; set; }
    }

    public class JudgeSubmissionScoreResponse
    {
        public Guid ScoreId { get; set; }
        public Guid SubmissionId { get; set; }
        public Guid AssignTrackId { get; set; }
        public Guid? RetakeFromScoreId { get; set; }
        public decimal? TotalScore { get; set; }
        public bool IsRetake { get; set; }
        public bool IsMock { get; set; }
        public List<JudgeScoreItemResponse> ScoreItems { get; set; } = [];
    }

    public class JudgeScoreItemResponse
    {
        public Guid CriteriaItemId { get; set; }
        public string CriteriaItemName { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public string? Comment { get; set; }
    }

    public class JudgeScoreDashboardResponse
    {
        public int TotalAssignedSubmissions { get; set; }
        public int TotalGradedSubmissions { get; set; }
        public int TotalPendingSubmissions { get; set; }
        public decimal GradedPercentage { get; set; }
    }

    public class JudgeStatusSubmissionResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? SubmissionId { get; set; }
        public SubmissionStatusEnum? SubmissionStatus { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public Guid? ScoreId { get; set; }
        public decimal? TotalScore { get; set; }
    }

    public class JudgeEventRoundSubmissionsResponse
    {
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public List<JudgeEventTrackSubmissionsResponse> Tracks { get; set; } = [];
    }

    public class JudgeEventTrackSubmissionsResponse
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public PaginationValue Submissions { get; set; } = new();
    }

    public class JudgeMyScoreItemResponse
    {
        public Guid ScoreId { get; set; }
        public Guid SubmissionId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public decimal? TotalScore { get; set; }
        public bool IsRetake { get; set; }
        public bool IsMock { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class JudgeTeamSubmissionInfo
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? SubmissionId { get; set; }
        public SubmissionStatusEnum? SubmissionStatus { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public bool IsGraded { get; set; }
    }

    public class JudgeTrackTeamResponse
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public List<JudgeTeamSubmissionInfo> Teams { get; set; } = new();
    }

    public class JudgeRoundTeamResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? SubmissionId { get; set; }
        public SubmissionStatusEnum? SubmissionStatus { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string GradingStatus { get; set; } = "Pending";
        public decimal? TotalScore { get; set; }
    }

    public class JudgeTeamSubmissionListResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public int? RoundNo { get; set; }
        public Guid RoundDetailId { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public SubmissionStatusEnum? Status { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string GradingStatus { get; set; } = "Pending";
        public Guid? ScoreId { get; set; }
        public decimal? TotalScore { get; set; }
    }

    public class JudgeRoundAllSubmissionResponse
    {
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public Guid? SubmissionId { get; set; }
        public string? Url { get; set; }
        public SubmissionStatusEnum? SubmissionStatus { get; set; }
        public DateTimeOffset? SubmittedAt { get; set; }
        public string GradingStatus { get; set; } = "Pending";
        public Guid? ScoreId { get; set; }
        public decimal? TotalScore { get; set; }
    }

    public class JudgeRegradeSubmissionResponse
    {
        public Guid SubmissionId { get; set; }
        public Guid RoundDetailId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string? TrackTitle { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public Guid ReportId { get; set; }
        public string? ReportTitle { get; set; }
        public Guid SourceScoreId { get; set; }
        public decimal? SourceTotalScore { get; set; }
        public bool IsRegraded { get; set; }
        public Guid? RegradeScoreId { get; set; }
        public decimal? RegradeTotalScore { get; set; }
        public DateTimeOffset ApprovedAt { get; set; }
    }
}
