namespace Hackathon.Application.Services.Lecturer.Event;

public class GetLecturerEventsResponse
{
    public List<LecturerEventItem> Events { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class LecturerEventItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetLecturerEventDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset? RegisterLimitTime { get; set; }
    public int? LimitTeam { get; set; }
    public int? MinMember { get; set; }
    public int? MaxMember { get; set; }
    public string? Status { get; set; }
    public bool IsDisable { get; set; }
    public int? NumberRound { get; set; }
    public string? Season { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
