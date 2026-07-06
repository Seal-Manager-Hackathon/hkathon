using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Teams : BaseEntity<Guid>, IAuditableEntity
{
    public required string Name { get; set; }
    public bool CanEdit { get; set; }

    public ICollection<TeamDetails> TeamDetails { get; set; } = new List<TeamDetails>();
    public ICollection<RegisterTeams> RegisterTeams { get; set; } = new List<RegisterTeams>();
    public ICollection<Invitations> Invitations { get; set; } = new List<Invitations>();
    public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
    public ICollection<LeaderBoardDetails> LeaderBoardDetails { get; set; } = new List<LeaderBoardDetails>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}