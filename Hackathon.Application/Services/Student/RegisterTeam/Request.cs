using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.RegisterTeam;

public class GetRegisterTeamsRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public int PageIndex { get; set; } = 1;
    [Range(1, 100, ErrorMessage = "PageSize Must Be Between 1 And 100")]
    public int PageSize { get; set; } = 10;
}

public class GetRegisterTeamsByTeamRequest
{
    public Guid TeamId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserEventsRequest
{
    public Guid UserId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
