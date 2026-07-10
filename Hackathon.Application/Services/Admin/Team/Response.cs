namespace Hackathon.Application.Services.Admin.Team;

public class GetUserTeamsResponse
{
    public List<UserTeamItem> Teams { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class UserTeamItem
{
    public Guid TeamDetailId { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public bool CanEdit { get; set; }
    public bool IsDisable { get; set; }
    public bool IsLeader { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}



public class GetTeamDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool CanEdit { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public List<TeamMemberItem> Members { get; set; } = new();
}

public class TeamMemberItem
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsLeader { get; set; }
    public string? Status { get; set; }
}

public class GetTeamsResponse
{
    public List<TeamCard> Teams { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TeamCard
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public bool CanEdit { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetTeamCountResponse
{
    public int Total { get; set; }
}

public class GetTeamEventsResponse
{
    public List<TeamEventItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TeamEventItem
{
    public Guid RegisterTeamId { get; set; }
    public Guid EventId { get; set; }
    public string EventName { get; set; } = null!;
    public string? Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
