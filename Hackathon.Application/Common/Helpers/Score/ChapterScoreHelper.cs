namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính điểm chapter cho 1 team trong 1 năm.
/// Điểm chapter = trung bình điểm các event trong cùng 1 năm (dựa trên EventScoreHelper).
/// Dùng Average để chuẩn hóa giữa các event có số round khác nhau.
/// </summary>
public static class ChapterScoreHelper
{
    /// <summary>
    /// Tính điểm chapter từ danh sách điểm các event trong năm.
    /// </summary>
    /// <param name="eventScores">Danh sách điểm từng event (đã tính từ EventScoreHelper)</param>
    /// <returns>Điểm chapter (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> eventScores)
    {
        if (eventScores.Count == 0)
            return 0;

        return Math.Round(eventScores.Average(), 2);
    }
}
