namespace Hackathon.Application.Services.Admin.Invitation;

public class GetInvitationsResponse
{
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public List<InvitationItem> Items { get; set; } = new();
}

public class InvitationItem
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = null!;
    public string UserFirstName { get; set; } = string.Empty;
    public string UserLastName { get; set; } = string.Empty;
    public string? UserAvatarUrl { get; set; }
    public string? Status { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? LimitTime { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
