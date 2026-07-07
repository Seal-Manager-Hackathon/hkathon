using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IAwardRepository
{
    Task<List<Awards>> GetByEventIdAsync(Guid eventId);
    Task<Awards?> GetByIdAsync(Guid id);
    Task AddAsync(Awards award);
    Task UpdateAsync(Awards award);
}
