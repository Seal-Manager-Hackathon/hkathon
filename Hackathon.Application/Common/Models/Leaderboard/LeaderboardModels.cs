namespace Hackathon.Application.Common.Models.Leaderboard;

// ── Round Leaderboard ──

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

// ── Event Leaderboard ──

public class GetEventLeaderboardResponse
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public int TotalRounds { get; set; }
    public List<EventLeaderboardItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class EventLeaderboardItem
{
    public int Rank { get; set; }
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public decimal EventScore { get; set; }
    public List<RoundScoreDetail> RoundScores { get; set; } = new();
}

public class RoundScoreDetail
{
    public int RoundNo { get; set; }
    public string RoundName { get; set; } = string.Empty;
    public decimal ScopeScore { get; set; }
}

// ── Chapter Leaderboard ──

public class GetChapterLeaderboardResponse
{
    public int Year { get; set; }
    public int EventCount { get; set; }
    public List<ChapterLeaderboardItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class ChapterLeaderboardItem
{
    public int Rank { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public decimal ChapterScore { get; set; }
    public int EventCount { get; set; }
    public List<EventScoreDetail> EventScores { get; set; } = new();
}

public class EventScoreDetail
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid RegisterTeamId { get; set; }
    public decimal EventScore { get; set; }
}
