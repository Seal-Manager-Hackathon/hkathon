using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính điểm round cho 1 submission.
/// Mỗi critical item = trung bình cộng các ScoreItems có cùng CriteriaItemId.
/// Score total = tổng các critical item scores (ghi đè vào Scores.TotalScore).
/// </summary>
public static class RoundScoreHelper
{
    /// <summary>
    /// Tính điểm cho 1 submission, trả về map {ScoreId, TotalScore}.
    /// </summary>
    public static Dictionary<Guid, decimal> Calculate(Submissions submission)
    {
        var results = new Dictionary<Guid, decimal>();

        foreach (var score in submission.Scores)
        {
            var totalScore = CalculateScoreTotal(score.ScoreItems.ToList());
            results[score.Id] = totalScore;
        }

        return results;
    }

    /// <summary>
    /// Tính TotalScore cho 1 Score từ list ScoreItems.
    /// Group by CriteriaItemId → avg mỗi group → sum các avg.
    /// </summary>
    private static decimal CalculateScoreTotal(List<ScoreItems> scoreItems)
    {
        var validItems = scoreItems.Where(si => si.Score.HasValue).ToList();
        if (validItems.Count == 0)
            return 0;

        var grouped = validItems.GroupBy(si => si.CriteriaItemId);

        decimal total = 0;
        foreach (var group in grouped)
        {
            total += group.Average(si => si.Score!.Value);
        }

        return Math.Round(total, 2);
    }
}
