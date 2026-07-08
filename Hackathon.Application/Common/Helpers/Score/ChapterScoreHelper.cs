namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính chapterScore cho 1 team trong 1 năm.
///
/// Term:
///   chapterScore = AVG(eventScore_j)
///   với j chạy qua các event team đã tham gia trong năm đó.
///
/// Dùng Average để chuẩn hóa giữa các event có số round khác nhau.
/// Không weighted — tất cả event trong năm có vai trò như nhau.
/// </summary>
public static class ChapterScoreHelper
{
    /// <summary>
    /// Tính chapterScore = trung bình eventScore các event team đã tham gia trong năm.
    /// </summary>
    /// <param name="eventScores">Danh sách eventScore từng event (đã tính từ EventScoreHelper)</param>
    /// <returns>chapterScore (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> eventScores)
    {
        if (eventScores.Count == 0)
            return 0;

        return Math.Round(eventScores.Average(), 2);
    }
}
