namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Tính eventScore cho 1 team trong 1 event.
///
/// Term:
///   eventScore = Σ(weight_i × roundScore_i) / Σ(weight_i)
///   Trong đó:
///     roundScore_i = scopeScore của team trong round thứ i
///                   (bằng 0 nếu team ko tham gia round đó)
///     weight_i     = trọng số round, mặc định = 1
///                    (Round entity ko có Weight field → tạm thời luôn = 1)
///
/// Config mặc định: weight_i = 1 cho tất cả round
/// → eventScore = Σ(roundScore_i) / totalRounds
///
/// Mẫu số tính trên tất cả round của event (kể cả round team ko tham gia, roundScore=0)
/// để tránh team chỉ tham gia 1 round vẫn có eventScore cao ko công bằng.
/// Khi nào Round entity có Weight field, chỉ cần đổi weight_i từ 1 sang giá trị thật.
/// </summary>
public static class EventScoreHelper
{
    /// <summary>
    /// Tính eventScore theo weighted average.
    /// </summary>
    /// <param name="roundScores">Danh sách roundScore của team (chỉ các round team ĐÃ THAM GIA)</param>
    /// <param name="totalRounds">Tổng số round của event (kể cả round team ko tham gia)</param>
    /// <returns>eventScore (làm tròn 2 chữ số)</returns>
    public static decimal Calculate(List<decimal> roundScores, int totalRounds)
    {
        if (totalRounds == 0)
            return 0;

        // weight_i = 1 cho tất cả round
        // roundScores chỉ chứa điểm round team đã tham gia
        // Σ(weight_i × roundScore_i) = Σ(roundScore_i)  (vì weight_i = 1)
        // Σ(weight_i)                 = totalRounds
        var sum = roundScores.Sum();
        return Math.Round(sum / totalRounds, 2);
    }
}
