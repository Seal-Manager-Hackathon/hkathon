using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Judge;

public class SubmitScoreRequest
{
    [Required]
    public List<ScoreItemInput> Scores { get; set; } = [];
}

public class ScoreItemInput
{
    [Required]
    public Guid CriteriaItemId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Score { get; set; }

    public string? Comment { get; set; }
}

public class UpdateScoreItemRequest
{
    public decimal? Score { get; set; }
    public string? Comment { get; set; }
}
