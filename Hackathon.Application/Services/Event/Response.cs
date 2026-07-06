namespace Hackathon.Application.Services.Event;

public class GetEventCountResponse
{
    public int Total { get; set; }
}

public class GetRecentEventsResponse
{
    public List<EventItem> Events { get; set; } = new();
}

public class EventItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? StartTime { get; set; }
    public DateTimeOffset? EndTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
