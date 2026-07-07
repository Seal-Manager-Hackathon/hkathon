using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ISubmissionRepository
{
    Task<(List<RoundDetails> Items, int TotalCount)> GetSubmissionsAsync(
        Guid eventId, Guid? roundId, Guid? trackId, Guid? topicId, Guid? registerTeamId,
        int pageIndex, int pageSize);
}
