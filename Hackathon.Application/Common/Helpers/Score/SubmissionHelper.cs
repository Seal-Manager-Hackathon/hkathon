using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Helper xử lý submission trong round — lấy submission cuối cùng của 1 team trong round.
/// </summary>
public static class SubmissionHelper
{
    /// <summary>
    /// Lấy submission cuối cùng (mới nhất) của 1 team trong round.
    /// </summary>
    public static Submissions? GetLastSubmission(RoundDetails roundDetail)
        => roundDetail.Submissions
            .OrderByDescending(s => s.SubmittedAt)
            .FirstOrDefault();
}
