namespace Hackathon.Application.Services.Admin.Leaderboard;

public interface ILeaderboardService
{
    Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize);
}