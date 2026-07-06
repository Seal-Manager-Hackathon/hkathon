using System;
using System.Collections.Generic;
using Hackathon.Repository.Enum;

namespace Hackathon.Service.AssignEvents;

public static class Response
{
    public class AssignEventResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? EventRoleId { get; set; }
        public Guid EventId { get; set; }
    }

    public class AssignLecturerDetailResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid? EventRoleId { get; set; }
        public EventRoleEnum? EventRole { get; set; }
        public RoleEnum Role { get; set; }
        public bool IsDisable { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<AssignedTrackInfo> AssignedTracks { get; set; } = new();
    }

    public class AssignedTrackInfo
    {
        public Guid AssignTrackId { get; set; }
        public Guid TrackId { get; set; }
        public string TrackTitle { get; set; } = string.Empty;
        public bool IsDisable { get; set; }
    }

    public class AvailableLecturerResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
