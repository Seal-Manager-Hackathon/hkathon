using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class LeaderBoardDetails : BaseEntity<Guid>, IAuditableEntity
{
    public Guid LeaderBoardId { get; set; }
    public Guid TeamId { get; set; }
    public decimal? Score { get; set; }
    public int? LevelAward { get; set; }

    public LeaderBoards LeaderBoard { get; set; } = null!;
    public Teams Team { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}