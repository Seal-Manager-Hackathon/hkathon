using Hackathon.Application.Common.Models.Leaderboard;

namespace Hackathon.Application.Services.Admin.Leaderboard;

public interface ILeaderboardService
{
    Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize);
    Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize);
    Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize);
    Task PublishChapter(int year);
    Task HideChapter(int year);
    Task PublishEvent(Guid eventId);
    Task HideEvent(Guid eventId);
}