namespace Hackathon.Service.LeaderBoards;

public static class Response
{
    public class YearLeaderboardResponse
    {
        public int Rank { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public decimal TotalYearScore { get; set; }
        public int EventsParticipated { get; set; }
    }
}
