namespace Hackathon.Application.Services.Student.Submission;

public class GetRegisterTeamSubmissionsResponse
{
    public List<RegisterTeamSubmissionItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class RegisterTeamSubmissionItem
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

// ── Submission Detail ──

public class SubmissionDetailResponse
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
    public SubmittedByInfo? SubmittedBy { get; set; }
    public decimal? TotalScore { get; set; }
    public int JudgeCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
