using Hackathon.Domain.Common;
using Hackathon.Domain.Enums.RegisterTeam;

namespace Hackathon.Domain.Entities;

public class RegisterTeams : BaseEntity<Guid>, IAuditableEntity
{
    public Guid TeamId { get; set; }
    public Guid EventId { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public string? Description { get; set; }
    public string? RejectionReason { get; set; }
    public RegisterTeamStatusEnum? Status { get; set; }
    public bool IsBanned { get; set; }

    public Teams Team { get; set; } = null!;
    public Events Event { get; set; } = null!;
    public Tracks? Track { get; set; }
    public Topics? Topic { get; set; }
    public ICollection<RoundDetails> RoundDetails { get; set; } = new List<RoundDetails>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}