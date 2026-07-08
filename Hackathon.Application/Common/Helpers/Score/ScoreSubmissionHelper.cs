using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;

namespace Hackathon.Application.Common.Helpers;

/// <summary>
/// Helper để chấm điểm 1 bài nộp (submission).
///
/// Tự động suy ra AssignTrackId từ user hiện tại (grader), event, track của submission.
/// Grader phải có EventRole = Judge và được assign track tương ứng.
/// </summary>
public static class ScoreSubmissionHelper
{
    /// <summary>
    /// Kiểm tra và lấy AssignTrack hợp lệ cho grader.
    /// </summary>
    public static async Task<AssignTracks> ValidateAndGetAssignTrackAsync(
        IAssignEventRepository assignEventRepository,
        Guid graderUserId,
        Guid eventId,
        Guid trackId)
    {
        var assignTrack = await assignEventRepository.GetGraderAssignTrackAsync(
            graderUserId, eventId, trackId);
        if (assignTrack == null)
            throw new BadRequestException(
                "You Are Not Assigned As Judge For This Track In This Event");

        return assignTrack;
    }

    /// <summary>
    /// Tạo Scores + ScoreItems từ template items.
    /// Các item không được nhập → mặc định Score = 0, Comment = null.
    /// TotalScore = SUM(Score của tất cả items).
    /// </summary>
    public static Scores CreateScore(
        Guid submissionId,
        Guid assignTrackId,
        bool isMock,
        List<CriteriaItems> templateItems,
        Dictionary<Guid, (decimal? Score, string? Comment)>? submittedItems)
    {
        var score = new Scores
        {
            Id = Guid.NewGuid(),
            SubmissionId = submissionId,
            AssignTrackId = assignTrackId,
            IsRetake = false,
            RetakeFromScoreId = null,
            TotalScore = null, // calculated below
            IsMock = isMock,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        submittedItems ??= new();
        decimal total = 0m;
        var scoreItems = new List<ScoreItems>();

        foreach (var templateItem in templateItems)
        {
            var hasInput = submittedItems.TryGetValue(templateItem.Id, out var input);
            var itemScore = hasInput && input.Score.HasValue ? input.Score.Value : 0m;

            scoreItems.Add(new ScoreItems
            {
                Id = Guid.NewGuid(),
                ScoreId = score.Id,
                CriteriaItemId = templateItem.Id,
                AssignTrackId = assignTrackId,
                Score = hasInput ? input.Score : 0m,
                Comment = hasInput ? input.Comment : null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });
            total += itemScore;
        }

        score.TotalScore = total;
        score.ScoreItems = scoreItems;
        return score;
    }
}
