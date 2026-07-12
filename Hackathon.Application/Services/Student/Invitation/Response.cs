namespace Hackathon.Application.Services.Student.Invitation;

public class GetInvitationsResponse
{
    public List<InvitationItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class InvitationItem
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public bool TeamCanEdit { get; set; }

    // Invited user info
    public Guid? InvitedUserId { get; set; }
    public string? InvitedUserEmail { get; set; }
    public string? InvitedUserFirstName { get; set; }
    public string? InvitedUserLastName { get; set; }
    public string? InvitedUserAvatarUrl { get; set; }

    // Sender (leader) info — for received invitations
    public Guid? SentByUserId { get; set; }
    public string? SentByEmail { get; set; }
    public string? SentByFirstName { get; set; }
    public string? SentByLastName { get; set; }
    public string? SentByAvatarUrl { get; set; }

    public string? Status { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? LimitTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
