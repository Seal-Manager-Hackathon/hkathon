using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class ScoreItems : BaseEntity<Guid>, IAuditableEntity
{
    public Guid ScoreId { get; set; }
    public Guid CriteriaItemId { get; set; }
    public Guid AssignTrackId { get; set; }
    public decimal? Score { get; set; }
    public string? Comment { get; set; }

    public Scores ScoreEntity { get; set; } = null!;
    public CriteriaItems CriteriaItem { get; set; } = null!;
    public AssignTracks AssignTrack { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}