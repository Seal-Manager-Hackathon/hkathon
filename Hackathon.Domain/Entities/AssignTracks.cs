using Hackathon.Domain.Common;

namespace Hackathon.Domain.Entities;

public class AssignTracks : BaseEntity<Guid>, IAuditableEntity
{
    public Guid AssignEventId { get; set; }
    public Guid TrackId { get; set; }

    public AssignEvents AssignEvent { get; set; } = null!;
    public Tracks Track { get; set; } = null!;
    public ICollection<ScoreItems> ScoreItems { get; set; } = new List<ScoreItems>();
    public ICollection<MentorNotifications> MentorNotifications { get; set; } = new List<MentorNotifications>();
    public ICollection<Scores> Scores { get; set; } = new List<Scores>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}