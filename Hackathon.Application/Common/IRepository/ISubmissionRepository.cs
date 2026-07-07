using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ISubmissionRepository
{
    Task<Submissions?> GetByIdAsync(Guid submissionId);
    Task<(List<RoundDetails> Items, int TotalCount)> GetSubmissionsAsync(
        Guid eventId, Guid? roundId, Guid? trackId, Guid? topicId, Guid? registerTeamId, string? keyword,
        int pageIndex, int pageSize);
    Task<(List<RoundSummaryItem> Items, int TotalCount)> GetRoundSummaryAsync(
        Guid roundId, int pageIndex, int pageSize);
}

public class RoundSummaryItem
{
    public Guid RoundId { get; set; }
    public Guid EventId { get; set; }
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string TeamName { get; set; } = null!;
    public Guid? TrackId { get; set; }
    public string? TrackTitle { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicTitle { get; set; }
    public Guid? LastSubmissionId { get; set; }
    public decimal? TotalScore { get; set; }
    public int? RoundNo { get; set; }
}
