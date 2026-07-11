namespace Hackathon.Application.Services.Lecturer.Topic;

public class GetTopicsByTrackResponse
{
    public List<TopicItem> Topics { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class TopicItem
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class GetTopicDetailResponse
{
    public Guid Id { get; set; }
    public Guid TrackId { get; set; }
    public string TrackTitle { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsDisable { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}