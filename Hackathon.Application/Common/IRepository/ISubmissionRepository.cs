using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ISubmissionRepository
{
    Task<Submissions?> GetByIdAsync(Guid submissionId);
    Task<(List<RoundDetails> Items, int TotalCount)> GetSubmissionsAsync(
        Guid eventId, Guid? roundId, Guid? trackId, Guid? topicId, Guid? registerTeamId, string? keyword,
        int pageIndex, int pageSize);
}
