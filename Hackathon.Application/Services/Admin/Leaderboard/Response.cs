namespace Hackathon.Application.Services.Admin.Leaderboard;

public class GetRoundLeaderboardResponse
{
    public Guid RoundId { get; set; }
    public string RoundName { get; set; } = null!;
    public Guid EventId { get; set; }
    public string EventName { get; set; } = null!;
    public List<RoundLeaderboardItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class RoundLeaderboardItem
{
    public int Rank { get; set; }
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public Guid? LastSubmissionId { get; set; }
    public decimal? TotalScore { get; set; }
}
