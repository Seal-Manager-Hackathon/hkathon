using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IRoundRepository
{
    Task<Rounds?> GetFirstRoundByEventIdAsync(Guid eventId);
    Task<int> CountTeamsInRoundAsync(Guid roundId);
    Task AddRoundDetailAsync(RoundDetails roundDetail);
}
