namespace Hackathon.Application.Services.Student.Track;

public class GetTracksByEventResponse
{
    public List<TrackItem> Tracks { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetTrackDetailResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public int RegisterTeamCount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class TrackItem
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
