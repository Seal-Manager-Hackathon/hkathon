namespace Hackathon.Application.Services.Judge;

public class JudgeTrackItem
{
    public Guid AssignTrackId { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public string? TrackDescription { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
}

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
    public string TeamName { get; set; } = null!;
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = null!;
    public int? RoundNo { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public Guid? SubmissionId { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public string GradingStatus { get; set; } = "Pending";
    public Guid? ScoreId { get; set; }
    public decimal? TotalScore { get; set; }
}
