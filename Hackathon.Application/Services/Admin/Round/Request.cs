namespace Hackathon.Application.Services.Admin.Round;

public class GetRoundsRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public int? RoundNo { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class CreateRoundRequest
{
    public Guid EventId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }
}

public class UpdateRoundRequest
{
    public Guid RoundId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? StartSubmission { get; set; }
    public DateTimeOffset? EndSubmission { get; set; }
    public int? LimitTeam { get; set; }
}

public class SwapRoundRequest
{
    public Guid EventId { get; set; }
    public Guid RoundId { get; set; }
    public int TargetRoundNo { get; set; }
}
