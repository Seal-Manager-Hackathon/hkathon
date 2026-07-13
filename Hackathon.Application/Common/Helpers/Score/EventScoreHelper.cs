namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính eventScore cho 1 team trong 1 event.
///
/// eventScore = SUM(roundScores) — tổng điểm tất cả các round team đã tham gia.
/// </summary>
public static class EventScoreHelper
{
    /// <summary>
    /// Tính eventScore = tổng điểm các round.
    /// </summary>
    /// <param name="roundScores">Danh sách roundScore của team (các round team đã tham gia)</param>
    /// <returns>eventScore (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> roundScores)
    {
        if (roundScores.Count == 0)
            return 0;

        return Math.Round(roundScores.Sum(), 2);
    }
}
