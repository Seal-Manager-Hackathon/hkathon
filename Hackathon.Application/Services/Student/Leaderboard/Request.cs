namespace Hackathon.Application.Services.Student.Leaderboard;

public class GetMyYearRankRequest
{
    public int Year { get; set; }
}

public class GetMyYearDetailRequest
{
    public int Year { get; set; }
}

public class GetMyEventRankRequest
{
    public Guid EventId { get; set; }
}
