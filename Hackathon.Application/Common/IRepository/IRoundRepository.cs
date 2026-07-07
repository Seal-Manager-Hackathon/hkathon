using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IRoundRepository
{
    Task<Rounds?> GetByIdAsync(Guid id);
    Task<Rounds?> GetFirstRoundByEventIdAsync(Guid eventId);
    Task<int> CountTeamsInRoundAsync(Guid roundId);
    Task AddRoundDetailAsync(RoundDetails roundDetail);
    Task<(List<Rounds> Items, int TotalCount)> SearchByEventIdAsync(
        Guid eventId, string? keyword, int? roundNo, bool? isDisable,
        int pageIndex, int pageSize);
    Task<int?> GetMaxRoundNoAsync(Guid eventId);
    Task AddAsync(Rounds round);
    Task UpdateAsync(Rounds round);
    Task<Rounds?> GetByEventIdAndRoundNoAsync(Guid eventId, int roundNo);
    Task<List<Rounds>> GetRoundsGreaterThanRoundNoAsync(Guid eventId, int roundNo);
}
