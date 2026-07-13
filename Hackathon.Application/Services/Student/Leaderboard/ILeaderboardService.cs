using Hackathon.Application.Common.Models.Leaderboard;

namespace Hackathon.Application.Services.Student.Leaderboard;

public interface ILeaderboardService
{
    /// <summary>
    /// API 1: GET /api/v1/student/events/chapter/{year}/leaderboard
    /// Chapter leaderboard — only published leaderboards, IsDisable=false
    /// </summary>
    Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize);

    /// <summary>
    /// API 2: GET /api/v1/student/events/{eventId}/leaderboard
    /// Event leaderboard — only if leaderboard IsDisable=false
    /// </summary>
    Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize);

    /// <summary>
    /// API 3: GET /api/v1/student/leaderboard/my-year-rank?year=2026
    /// My team's rank in chapter year leaderboard
    /// </summary>
    Task<GetMyYearRankResponse> GetMyYearRank(GetMyYearRankRequest request);

    /// <summary>
    /// API 4: GET /api/v1/student/leaderboard/my-year-detail?year=2026
    /// My team's year detail with per-event breakdown and event rank
    /// </summary>
    Task<GetMyYearDetailResponse> GetMyYearDetail(GetMyYearDetailRequest request);

    /// <summary>
    /// API 5: GET /api/v1/student/events/{eventId}/leaderboard/my-rank
    /// My team's rank in event leaderboard
    /// </summary>
    Task<GetMyEventRankResponse> GetMyEventRank(GetMyEventRankRequest request);
}
