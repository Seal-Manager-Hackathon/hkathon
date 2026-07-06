using System;
using Hackathon.Repository.Enum;

namespace Hackathon.Service.Lecturers;

public static class Response
{
    public class LecturerEventResponse
    {
        public Guid AssignEventId { get; set; }
        public Guid EventId { get; set; }
        public string EventName { get; set; } = null!;
        public Hackathon.Repository.Enum.SeasonEnum? Season { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public EventRoleEnum? Role { get; set; }
        public EventStatusEnum? EventStatus { get; set; }
        public int? Year { get; set; }
    }

    public class LecturerTrackResponse
    {
        public Guid AssignTrackId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public string? TrackDescription { get; set; }
        public int? MaxTeam { get; set; }
    }

    public class LecturerEventTracksResponse
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public EventRoleEnum? Role { get; set; }
        public List<LecturerTrackResponse> Tracks { get; set; } = new();
    }
}
