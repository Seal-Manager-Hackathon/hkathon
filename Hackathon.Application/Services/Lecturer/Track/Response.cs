namespace Hackathon.Application.Services.Lecturer.Track;

public class GetTracksByEventResponse
{
    public List<TrackItem> Tracks { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TrackItem
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetMyAssignedTracksResponse
{
    public List<MyAssignedTrackItem> Tracks { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class MyAssignedTrackItem
{
    public Guid Id { get; set; }            // trackId
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public Guid EventRoleId { get; set; }
    public string EventRoleName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetTrackDetailResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? MaxTeam { get; set; }
    public bool IsDisable { get; set; }
    public int RegisterTeamCount { get; set; }
    public Guid EventRoleId { get; set; }
    public string EventRoleName { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}