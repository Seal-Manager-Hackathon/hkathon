using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IRoundRepository
{
    Task<Rounds?> GetFirstRoundByEventIdAsync(Guid eventId);
    Task<int> CountTeamsInRoundAsync(Guid roundId);
    Task AddRoundDetailAsync(RoundDetails roundDetail);
    Task<(List<Rounds> Items, int TotalCount)> SearchByEventIdAsync(
        Guid eventId, string? keyword, int? roundNo,
        int pageIndex, int pageSize);
    Task<int?> GetMaxRoundNoAsync(Guid eventId);
    Task AddAsync(Rounds round);
    Task<Rounds?> GetByEventIdAndRoundNoAsync(Guid eventId, int roundNo);
}
