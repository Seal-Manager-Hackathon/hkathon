using Hackathon.Application.Common.Models.Leaderboard;

namespace Hackathon.Application.Services.Lecturer.Leaderboard;

public interface ILeaderboardService
{
    Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize);
    Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize);
    Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize);
}
