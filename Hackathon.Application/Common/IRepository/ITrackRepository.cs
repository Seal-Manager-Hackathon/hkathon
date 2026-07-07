using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITrackRepository
{
    Task<Tracks?> GetByIdAsync(Guid id);
    Task<List<Tracks>> GetByEventIdAsync(Guid eventId);
    Task AddAsync(Tracks track);
}
