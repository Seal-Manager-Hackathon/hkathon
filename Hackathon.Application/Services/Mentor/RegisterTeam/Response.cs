namespace Hackathon.Application.Services.Mentor.RegisterTeam;

public class GetRegisterTeamByTrackResponse
{
    public List<RegisterTeamByTrackItem> RegisterTeams { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class RegisterTeamByTrackItem
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid? TrackId { get; set; }
    public string? TrackName { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }
    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisable { get; set; }
    public Guid? RoundId { get; set; }
    public string? RoundName { get; set; }
    public int? RoundNo { get; set; }
}
