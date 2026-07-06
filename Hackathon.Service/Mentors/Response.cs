using Hackathon.Repository.Enum;

namespace Hackathon.Service.Mentors;

public static class Response
{
    public class MentorEventResponse
    {
        public Guid AssignEventId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public Hackathon.Repository.Enum.EventRoleEnum? Role { get; set; }
    }

    public class MentorTrackResponse
    {
        public Guid AssignTrackId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public string? TrackDescription { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
    }

    public class MentorTrackTeamResponse
    {
        public Guid RegisterTeamId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid? TopicId { get; set; }
        public string? TopicTitle { get; set; }
        public string? LeaderName { get; set; }
        public int MemberCount { get; set; }
    }

    public class MentorNotificationResponse
    {
        public Guid MentorNotificationId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
