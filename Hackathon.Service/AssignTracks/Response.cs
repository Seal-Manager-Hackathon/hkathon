using System;
using Hackathon.Repository.Enum;

namespace Hackathon.Service.AssignTracks;

public static class Response
{
    public class AssignTrackLecturerResponse
    {
        public Guid Id { get; set; }
        public Guid AssignEventId { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public EventRoleEnum? EventRole { get; set; }
        public RoleEnum Role { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class AssignTrackResponse
    {
        public Guid Id { get; set; }
        public Guid AssignEventId { get; set; }
        public Guid TrackId { get; set; }
    }
}
