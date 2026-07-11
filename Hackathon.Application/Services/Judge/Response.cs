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
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public SubmittedByInfo? SubmittedBy { get; set; }
    public LastSubmissionInfo? LastSubmission { get; set; }
    public string GradingStatus { get; set; } = "Pending";
    public Guid? ScoreId { get; set; }
    public decimal? TotalScore { get; set; }
}

public class SubmittedByInfo
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public class LastSubmissionInfo
{
    public Guid Id { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}

// Criteria
public class SubmissionCriteriaResponse
{
    public Guid SubmissionId { get; set; }  // extra for judge context
    public Guid Id { get; set; }            // templateId
    public Guid RoundId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public bool IsActive { get; set; }
    public List<CriteriaItemDetail> Items { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class CriteriaItemDetail
{
    public Guid Id { get; set; }
    public Guid CriteriaTemplateId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
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

// Update Score response với pagination + isUpdated
public class UpdateScoreResponse
{
    public Guid ScoreId { get; set; }
    public List<UpdatedScoreItemResponse> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class UpdatedScoreItemResponse
{
    public Guid ScoreItemId { get; set; }
    public Guid ScoreId { get; set; }
    public Guid SubmissionId { get; set; }
    public Guid CriteriaItemId { get; set; }
    public string CriteriaItemName { get; set; } = string.Empty;
    public decimal? Score { get; set; }
    public string? Comment { get; set; }
    public Guid? GradedByUserId { get; set; }
    public bool IsUpdated { get; set; }
}

// RegisterTeam Submissions — single latest submission per register team
public class GetRegisterTeamSubmissionsResponse
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public SubmittedByInfo? SubmittedBy { get; set; }
    public LastSubmissionInfo? LastSubmission { get; set; }
    public string GradingStatus { get; set; } = "Pending";
    public Guid? ScoreId { get; set; }
    public decimal? TotalScore { get; set; }
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
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? RoundId { get; set; }
    public string? RoundName { get; set; }
    public Guid? SubmissionId { get; set; }
    public string? Url { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string GradingStatus { get; set; } = "Pending";
    // Score info — null nếu chưa chấm
    public Guid? ScoreId { get; set; }
    public decimal? TotalScore { get; set; }
}
