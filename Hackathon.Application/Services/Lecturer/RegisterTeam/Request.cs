namespace Hackathon.Application.Services.Lecturer.RegisterTeam;

public class GetRegisterTeamsRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public bool? IsBanned { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetRegisterTeamsByTeamRequest
{
    public string? Status { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
