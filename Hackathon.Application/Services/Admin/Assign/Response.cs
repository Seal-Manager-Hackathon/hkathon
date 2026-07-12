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

public class GetUserAssignEventsResponse
{
    public List<UserAssignEventItem> Events { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class UserAssignEventItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public int? NumberRound { get; set; }
    public string? Season { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public Guid? EventRoleId { get; set; }
    public string? EventRoleName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDisable { get; set; }
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
