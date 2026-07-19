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

public class InvitationDetailResponse
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public int TeamMemberCount { get; set; }
    public bool TeamCanEdit { get; set; }
    public List<InvitationTeamMemberItem> TeamMembers { get; set; } = new();

    // Invited user info
    public Guid InvitedUserId { get; set; }
    public string? InvitedUserEmail { get; set; }
    public string? InvitedUserFirstName { get; set; }
    public string? InvitedUserLastName { get; set; }
    public string? InvitedUserAvatarUrl { get; set; }

    // Sender (leader) info
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

public class InvitationTeamMemberItem
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? AvatarUrl { get; set; }
    public bool IsLeader { get; set; }
}

public class InvitationTeamDetailResponse
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public int MemberCount { get; set; }
    public bool CanEdit { get; set; }
    public List<InvitationTeamMemberItem> Members { get; set; } = new();
}
