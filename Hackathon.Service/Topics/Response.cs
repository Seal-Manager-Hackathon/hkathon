namespace Hackathon.Service.Topics;

public static class Response
{
    public class AssignedTopicResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid EventId { get; set; }
        public Guid? TrackId { get; set; }
        public string? TrackTitle { get; set; }
        public string? TrackDescription { get; set; }
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? TopicDescription { get; set; }
    }

    public class TopicDetailResponse
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class CreateTopicResponse
    {
        public Guid Id { get; set; }
        public Guid TrackId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
