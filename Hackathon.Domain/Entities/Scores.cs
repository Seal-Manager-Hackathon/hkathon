using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class Scores : BaseEntity<Guid>, IAuditableEntity
{
    public Guid SubmissionId { get; set; }
    public Guid AssignTrackId { get; set; }
    public bool IsRetake { get; set; }
    public Guid? RetakeFromScoreId { get; set; }
    public decimal? TotalScore { get; set; }
    public bool IsMock { get; set; }

    public Submissions Submission { get; set; } = null!;
    public AssignTracks AssignTrack { get; set; } = null!;
    public Scores? RetakeFromScore { get; set; }
    public ICollection<Scores> RetakeScores { get; set; } = new List<Scores>();
    public ICollection<ScoreItems> ScoreItems { get; set; } = new List<ScoreItems>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}