namespace Hackathon.Application.Services.Admin.Score;

public class GetSubmissionScoresResponse
{
    public Guid SubmissionId { get; set; }
    public List<ScoreDetail> Scores { get; set; } = new();
}

public class ScoreDetail
{
    public Guid ScoreId { get; set; }
    public Guid AssignTrackId { get; set; }
    public string? TrackTitle { get; set; }
    public decimal? TotalScore { get; set; }
    public bool IsRetake { get; set; }
    public bool IsMock { get; set; }
    public List<ScoreItemDetail> Items { get; set; } = new();
}

public class ScoreItemDetail
{
    public Guid ScoreItemId { get; set; }
    public Guid CriteriaItemId { get; set; }
    public string CriteriaName { get; set; } = null!;
    public decimal? Score { get; set; }
    public string? Comment { get; set; }
}

public class GetScoreItemsResponse
{
    public Guid ScoreId { get; set; }
    public List<ScoreItemDetail> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
