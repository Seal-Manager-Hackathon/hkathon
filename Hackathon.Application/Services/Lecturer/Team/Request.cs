namespace Hackathon.Application.Services.Lecturer.Team;

public class GetTeamsRequest
{
    public string? Keyword { get; set; }
    public bool? CanEdit { get; set; }
    public bool? IsDisable { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

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