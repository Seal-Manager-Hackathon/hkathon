namespace Hackathon.Application.Services.Staff.Score;

public class GetScoreDetailResponse
{
    public Guid ScoreId { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid AssignTrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public decimal? TotalScore { get; set; }
    public bool IsRetake { get; set; }
    public Guid? RetakeFromScoreId { get; set; }
    public bool IsMock { get; set; }
    public GraderInfo? GradedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetSubmissionGraderScoresResponse
{
    public Guid SubmissionId { get; set; }
    public List<ScoreDetail> Scores { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class ScoreDetail
{
    public Guid ScoreId { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid AssignTrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public decimal? TotalScore { get; set; }
    public bool IsRetake { get; set; }
    public Guid? RetakeFromScoreId { get; set; }
    public bool IsMock { get; set; }
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
    public string? TrackTitle { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetScoreItemsResponse
{
    public Guid ScoreId { get; set; }
    public List<ScoreItemDetail> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetTeamRoundScoreResponse
{
    public Guid RoundId { get; set; }
    public Guid RegisterTeamId { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public decimal TotalScore { get; set; }
    public Guid? SubmissionId { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public bool IsLastSubmission { get; set; }
}
