namespace Hackathon.Application.Services.Student.Leaderboard;

// ── API 3: My Year Rank (chapter rank of user's team) ──

public class GetMyYearRankResponse
{
    public int Year { get; set; }
    public List<MyYearRankItem> Teams { get; set; } = new();
}

public class MyYearRankItem
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int Rank { get; set; }
    public decimal ChapterScore { get; set; }
    public int EventCount { get; set; }
}

// ── API 4: My Year Detail (per-event breakdown with event rank) ──

public class GetMyYearDetailResponse
{
    public int Year { get; set; }
    public List<MyYearDetailTeamItem> Teams { get; set; } = new();
}

public class MyYearDetailTeamItem
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int Rank { get; set; }
    public decimal ChapterScore { get; set; }
    public List<EventScoreRankDetail> EventScores { get; set; } = new();
}

public class EventScoreRankDetail
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Guid RegisterTeamId { get; set; }
    public decimal EventScore { get; set; }
    public int EventRank { get; set; }
}

// ── API 5: My Event Rank (user's team rank in event) ──

public class GetMyEventRankResponse
{
    public Guid EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public List<MyEventRankItem> Teams { get; set; } = new();
}

public class MyEventRankItem
{
    public int Rank { get; set; }
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public decimal EventScore { get; set; }
    public List<Common.Models.Leaderboard.RoundScoreDetail> RoundScores { get; set; } = new();
}
