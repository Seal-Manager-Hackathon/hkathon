namespace Hackathon.Service.Judges;

public static class Request
{
    public class SubmitScoreRequest
    {
        public decimal TotalScore { get; set; }
        public List<ScoreItemRequest> Scores { get; set; } = [];
    }

    public class ScoreItemRequest
    {
        public Guid CriteriaItemId { get; set; }
        public decimal Score { get; set; }
        public string? Comment { get; set; }
    }

    public class UpdateScoreItemRequest
    {
        public decimal? Score { get; set; }
        public string? Comment { get; set; }
    }
}
