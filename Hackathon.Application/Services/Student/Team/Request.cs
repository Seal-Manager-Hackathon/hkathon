namespace Hackathon.Application.Services.Student.Team;

public class GetTeamCountRequest
{
    public bool? IsDisable { get; set; }
}

public class GetTeamEventsRequest
{
    public Guid TeamId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
