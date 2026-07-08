namespace Hackathon.Application.Services.Admin.Submission;

public class GetSubmissionDetailResponse
{
    public Guid Id { get; set; }
    public Guid RoundDetailId { get; set; }
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = null!;
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public bool IsRegrade { get; set; }
    public SubmittedByUser? SubmittedBy { get; set; }
    public decimal? TotalScore { get; set; }
    public int JudgeCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class SubmissionScoreDetail
{
    public Guid ScoreId { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid AssignTrackId { get; set; }
    public string? TrackTitle { get; set; }
    public decimal? TotalScore { get; set; }
    public bool IsRetake { get; set; }
    public Guid? RetakeFromScoreId { get; set; }
    public bool IsMock { get; set; }
    public List<ScoreItemDetail> Items { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class ScoreItemDetail
{
    public Guid ScoreItemId { get; set; }
    public Guid ScoreId { get; set; }
    public Guid CriteriaItemId { get; set; }
    public Guid AssignTrackId { get; set; }
    public Guid AssignEventId { get; set; }
    public string CriteriaName { get; set; } = null!;
    public decimal? Score { get; set; }
    public string? Comment { get; set; }
    public GraderInfo? GradedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GraderInfo
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class GetSubmissionsResponse
{
    public List<SubmissionItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class SubmissionItem
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid EventId { get; set; }
    public string EventName { get; set; } = null!;
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = null!;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public SubmittedByUser? SubmittedBy { get; set; }
    public SubmissionRecordDto? LastSubmission { get; set; }
    public List<SubmissionRecordDto> Records { get; set; } = new();
}

public class SubmittedByUser
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class SubmissionRecordDto
{
    public Guid Id { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
