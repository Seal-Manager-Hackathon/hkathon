namespace Hackathon.Application.Services.Student.PopularEvent;

public class GetPopularEventsResponse
{
    public List<PopularEventItem> Events { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class PopularEventItem
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
    public int? NumberRound { get; set; }
    public string? Season { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public int ApprovedRegisterTeamCount { get; set; }
}
