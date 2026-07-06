namespace Hackathon.Service.LeaderBoards;

public static class Request
{
    public class AssignAwardRequest
    {
        public decimal? Score { get; set; }
        public int? LevelAward { get; set; }
    }
}
