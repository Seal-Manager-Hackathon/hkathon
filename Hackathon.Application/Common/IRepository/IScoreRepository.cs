using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IScoreRepository
{
    Task<Scores?> GetByIdAsync(Guid scoreId);
    Task<List<Scores>> GetBySubmissionIdAsync(Guid submissionId);
    Task<(List<ScoreItems> Items, int TotalCount)> GetScoreItemsByScoreIdAsync(Guid scoreId, int pageIndex, int pageSize);
}
