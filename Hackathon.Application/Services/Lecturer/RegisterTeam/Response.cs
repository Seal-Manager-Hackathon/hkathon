namespace Hackathon.Application.Services.Lecturer.RegisterTeam;

public class GetRegisterTeamsResponse
{
    public List<RegisterTeamCard> RegisterTeams { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class RegisterTeamCard
{
    public Guid Id { get; set; }
    public Guid RegisterTeamId => Id;
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public Guid? TrackId { get; set; }
    public string? TrackName { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }
    public string? Description { get; set; }
    public string? RejectionReason { get; set; }
    public string? Status { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisable { get; set; }
    public Guid? RoundId { get; set; }
    public string? RoundName { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class RegisterTeamDetailResponse
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? RejectionReason { get; set; }
    public string? Status { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisable { get; set; }
    public Guid? RoundId { get; set; }
    public string? RoundName { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public string? EventDescription { get; set; }
    public DateTimeOffset? EventStartDate { get; set; }
    public DateTimeOffset? EventEndDate { get; set; }

    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public bool TeamCanEdit { get; set; }
    public bool TeamIsDisable { get; set; }
    public DateTimeOffset TeamCreatedAt { get; set; }

    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }

    public List<RegisterTeamMemberItem> Members { get; set; } = new();
}

public class RegisterTeamMemberItem
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsLeader { get; set; }
    public string? Status { get; set; }
}

public class GetCompetitionStatusResponse
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public Guid? TrackId { get; set; }
    public string? TrackName { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }
    public Guid? CurrentRoundId { get; set; }
    public string? CurrentRoundName { get; set; }
    public int? CurrentRoundNo { get; set; }
    public Guid? MaxRoundId { get; set; }
    public string? MaxRoundName { get; set; }
    public int? MaxRoundNo { get; set; }
    public bool IsStillCompeting { get; set; }
}

public class GetUserEventsResponse
{
    public List<UserEventItem> Events { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetRegisterTeamsByTrackResponse
{
    public List<RegisterTeamByTrackItem> RegisterTeams { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class RegisterTeamByTrackItem
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public Guid? TrackId { get; set; }
    public string? TrackName { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisable { get; set; }
    public Guid? RoundId { get; set; }
    public string? RoundName { get; set; }
    public int? RoundNo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class UserEventItem
{
    public Guid RegisterTeamId { get; set; }
    public string? Status { get; set; }
    public bool IsBanned { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Guid EventId { get; set; }
    public string? EventName { get; set; }
    public string? EventDescription { get; set; }
    public DateTimeOffset? EventStartTime { get; set; }
    public DateTimeOffset? EventEndTime { get; set; }
    public string? EventStatus { get; set; }

    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }

    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
}