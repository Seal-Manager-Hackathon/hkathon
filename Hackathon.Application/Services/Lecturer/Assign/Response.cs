namespace Hackathon.Application.Services.Lecturer.Assign;

public class GetAssignedUsersResponse
{
    public List<AssignedUserItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class AssignedUserItem
{
    public Guid AssignEventId { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? EventRole { get; set; }
    public bool IsDisable { get; set; }
    public List<AssignedTrackItem>? AssignTracks { get; set; }
}

public class AssignedTrackItem
{
    public Guid TrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public bool IsDisable { get; set; }
}
