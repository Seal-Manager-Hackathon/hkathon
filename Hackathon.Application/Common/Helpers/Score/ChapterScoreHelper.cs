namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính chapterScore cho 1 team trong 1 năm.
///
/// chapterScore = SUM(eventScores) — tổng điểm tất cả các event team đã tham gia trong năm.
/// </summary>
public static class ChapterScoreHelper
{
    /// <summary>
    /// Tính chapterScore = tổng eventScore các event team đã tham gia trong năm.
    /// </summary>
    /// <param name="eventScores">Danh sách eventScore từng event (đã tính từ EventScoreHelper)</param>
    /// <returns>chapterScore (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> eventScores)
    {
        if (eventScores.Count == 0)
            return 0;

        return Math.Round(eventScores.Sum(), 2);
    }
}
