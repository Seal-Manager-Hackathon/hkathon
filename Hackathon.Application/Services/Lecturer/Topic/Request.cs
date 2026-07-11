namespace Hackathon.Application.Services.Lecturer.Topic;

public class GetTopicsByTrackRequest
{
    public Guid TrackId { get; set; }
    public string? Keyword { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}