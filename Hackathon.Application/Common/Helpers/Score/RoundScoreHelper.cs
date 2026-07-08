using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính scopeScore cho 1 submission trong 1 round.
///
/// Term:
///   judgeScore  = ScoreItems.Score            (điểm 1 judge chấm)
///   criteriaAvg = AVG(judgeScore) GROUP BY CriteriaItemId (trung bình các judge chấm cùng tiêu chí)
///   scopeScore  = SUM(criteriaAvg)            (ghi đè vào Scores.TotalScore)
///
/// Count: chỉ tính judgeScore có tồn tại (Score.HasValue = true),
/// ko yêu cầu bắt buộc tất cả judge được phân công phải chấm —
/// phòng trường hợp judge có việc bận ko chấm = ko có ScoreItem.
/// </summary>
public static class RoundScoreHelper
{
    /// <summary>
    /// Tính scopeScore cho 1 submission, trả về map {ScoreId, scopeScore}.
    /// </summary>
    public static Dictionary<Guid, decimal> Calculate(Submissions submission)
    {
        var results = new Dictionary<Guid, decimal>();

        foreach (var score in submission.Scores)
        {
            var totalScore = CalculateScopeScore(score.ScoreItems.ToList());
            results[score.Id] = totalScore;
        }

        return results;
    }

    /// <summary>
    /// Tính scopeScore (= Scores.TotalScore) từ list judgeScore.
    /// criteriaAvg = AVG(judgeScore) GROUP BY CriteriaItemId
    /// scopeScore  = SUM(criteriaAvg)
    ///
    /// Chỉ tính judgeScore có Score.HasValue = true (judge đã chấm thực tế).
    /// </summary>
    private static decimal CalculateScopeScore(List<ScoreItems> scoreItems)
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
