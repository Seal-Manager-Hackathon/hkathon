namespace Hackathon.Application.Services.Judge;

// Tracks
public class JudgeTrackItem
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

// Submissions
public class GetTrackSubmissionsResponse
{
    public List<TrackSubmissionItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TrackSubmissionItem
{
    public Guid? SubmissionId { get; set; }
    public Guid RoundDetailId { get; set; }
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string GradingStatus { get; set; } = "Pending";
    public Guid? ScoreId { get; set; }
    public decimal? TotalScore { get; set; }
}

// Criteria
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

// Score
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

// My Scores
public class GetMyScoresResponse
{
    public List<JudgeMyScoreItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class JudgeMyScoreItem
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
