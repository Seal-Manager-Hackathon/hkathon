using Hackathon.Repository.Enum;

namespace Hackathon.Service.Staff;

public static class Response
{
    public class StaffEventResponse
    {
        public Guid AssignEventId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public EventRoleEnum? Role { get; set; }
        public EventStatusEnum? EventStatus { get; set; }
        public int? Year { get; set; }
    }

    public class StaffReportListItemResponse
    {
        public Guid ReportId { get; set; }
        public Guid? SubmissionId { get; set; }
        public string? TeamName { get; set; }
        public string? EventName { get; set; }
        public string? Title { get; set; }
        public string? TypeReport { get; set; }
        public ReportStatusEnum? Status { get; set; }
        public string? StatusName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
 
    public class StaffReportDetailResponse : StaffReportListItemResponse
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid? AssignEventId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? RoundId { get; set; }
        public int? RoundNo { get; set; }
        public string? Description { get; set; }
        public string? ImgUrl { get; set; }
        public string? FileUrl { get; set; }
        public string? Reason { get; set; }
        public bool IsRegrade { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class ApproveRegradeResponse
    {
        public Guid ReportId { get; set; }
        public Guid? SubmissionId { get; set; }
        public ReportStatusEnum Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public bool IsRegrade { get; set; }
    }

    public class StaffRegradeSubmissionResponse
    {
        public Guid? SubmissionId { get; set; }
        public Guid RoundDetailId { get; set; }
        public string RoundName { get; set; } = string.Empty;
        public int? RoundNo { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public Guid ReportId { get; set; }
        public string? ReportTitle { get; set; }
        public string RegradeStatus { get; set; } = string.Empty;
        public DateTimeOffset ApprovedAt { get; set; }
        public List<SourceScoreRegradeResponse> SourceScores { get; set; } = new();
    }

    public class SourceScoreRegradeResponse
    {
        public Guid ScoreId { get; set; }
        public Guid JudgeId { get; set; }
        public string JudgeName { get; set; } = string.Empty;
        public decimal? TotalScore { get; set; }
        public bool HasRegraded { get; set; }
        public Guid? RegradeScoreId { get; set; }
        public decimal? RegradeTotalScore { get; set; }
        public DateTimeOffset? RegradedAt { get; set; }
    }
}
