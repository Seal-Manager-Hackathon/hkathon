namespace Hackathon.Application.Services.Mentor.Score;

public class GetRegisterTeamScoresResponse
{
    public Guid RegisterTeamId { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public List<RoundScoreItem> Rounds { get; set; } = new();
}

public class RoundScoreItem
{
    public Guid RoundId { get; set; }
    public int RoundNo { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public decimal? TotalScore { get; set; }
    public Guid? SubmissionId { get; set; }
    public string? SubmissionUrl { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }
    public int JudgeCount { get; set; }
}
