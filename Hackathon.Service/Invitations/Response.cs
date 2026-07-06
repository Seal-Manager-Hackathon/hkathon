using System;

namespace Hackathon.Service.Invitations;

public static class Response
{
    public class InvitationItemResponse
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public Hackathon.Repository.Enum.InvitationStatusEnum? Status { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? LimitTime { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? LeaderName { get; set; }
    }
}
