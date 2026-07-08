namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính điểm event cho 1 register team.
/// Điểm event = tổng trung bình điểm các round (dựa trên TotalScore của Scores).
/// </summary>
public static class EventScoreHelper
{
    /// <summary>
    /// Tính điểm event từ danh sách điểm các round.
    /// </summary>
    /// <param name="roundScores">Danh sách TotalScore của từng round (đã tính từ RoundScoreHelper)</param>
    /// <returns>Điểm event (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> roundScores)
    {
        if (roundScores.Count == 0)
            return 0;

        return Math.Round(roundScores.Average(), 2);
    }
}
