using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính scopeScore cho 1 submission trong 1 round.
///
/// Term:
///   Scores.TotalScore = tổng ScoreItems của 1 judge (đã tính khi chấm)
///   scopeScore        = SUM(Scores.TotalScore) của tất cả judge
///
/// Count: chỉ tính judge đã chấm thực tế (TotalScore.HasValue = true),
/// bỏ qua judge chưa chấm (ko có Scores). Điểm 0 vẫn được tính.
/// </summary>
public static class RoundScoreHelper
{
    /// <summary>
    /// Tính scopeScore = SUM(Scores.TotalScore) của submission.
    /// Trả về cả detail từng judge và tổng scopeScore.
    /// </summary>
    public static (Dictionary<Guid, decimal> JudgeScores, decimal Total) Calculate(Submissions submission)
    {
        var judgeScores = new Dictionary<Guid, decimal>();

        foreach (var score in submission.Scores)
        {
            if (!score.TotalScore.HasValue) continue;
            judgeScores[score.Id] = score.TotalScore.Value;
        }

        var total = Math.Round(judgeScores.Values.Sum(), 2);
        return (judgeScores, total);
    }
}
