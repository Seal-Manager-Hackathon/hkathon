namespace Hackathon.Application.Services.Admin.Assign;

public class GetAvailableUserResponse
{
    public List<AvailableUserItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

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
    public List<AssignedTrackItem>? AssignTracks { get; set; }
}

public class AssignedTrackItem
{
    public Guid TrackId { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class AvailableUserItem
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? College { get; set; }
    public string? PhoneNumber { get; set; }
}
