namespace Hackathon.Application.Services.Admin.Score;

public class ScoreSubmissionRequest
{
    public bool IsMock { get; set; }
    public List<ScoreItemInput> ScoreItems { get; set; } = new();
}

public class ScoreItemInput
{
    public Guid CriteriaItemId { get; set; }
    public decimal? Score { get; set; }
    public string? Comment { get; set; }
}
