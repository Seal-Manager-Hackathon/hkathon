using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class LeaderBoards : BaseEntity<Guid>, IAuditableEntity
{
    public Guid EventId { get; set; }
    public int? Year { get; set; }
    public bool IsLocked { get; set; }
    public bool IsPublished { get; set; }

    public Events Event { get; set; } = null!;
    public ICollection<LeaderBoardDetails> LeaderBoardDetails { get; set; } = new List<LeaderBoardDetails>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}