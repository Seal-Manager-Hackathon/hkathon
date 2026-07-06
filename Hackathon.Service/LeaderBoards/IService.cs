namespace Hackathon.Service.LeaderBoards;

public interface IService
{
    // #{Public}
    Task<List<Response.YearLeaderboardResponse>> GetYearLeaderboard(int year);

    // #{Staff} #{Admin}
    Task<string> AssignAward(Guid leaderBoardId, Guid teamId, Request.AssignAwardRequest request);
}
