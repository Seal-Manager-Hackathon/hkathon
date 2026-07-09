namespace Hackathon.Application.Services.Staff.RegisterTeam;

public class GetRegisterTeamsByTeamRequest
{
    public Guid TeamId { get; set; }
    public string? Status { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserEventsRequest
{
    public Guid UserId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class RejectRegisterTeamRequest
{
    public string? RejectionReason { get; set; }
}

public class BanRegisterTeamRequest
{
    public string RejectionReason { get; set; } = null!;
}

public class AssignTrackTopicRequest
{
    public Guid TrackId { get; set; }
    public Guid? TopicId { get; set; }
}

public class UpdateRegisterTeamRequest
{
    public Guid RegisterTeamId { get; set; }
    public string? Description { get; set; }
    public string? RejectionReason { get; set; }
    public string? Status { get; set; }
    public bool? IsBanned { get; set; }
    public bool? IsDisable { get; set; }
}

public class GetRegisterTeamsRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public bool? IsBanned { get; set; }
    public bool? IsDisable { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
