using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITrackRepository
{
    Task<Tracks?> GetByIdAsync(Guid id);
    Task<List<Tracks>> GetByEventIdAsync(Guid eventId);
    Task AddAsync(Tracks track);
    Task<(List<Tracks> Items, int TotalCount)> SearchByEventIdAsync(Guid eventId, string? keyword, bool? isDisable, int pageIndex, int pageSize);
    Task<(List<Tracks> Items, int TotalCount)> GetByEventIdAsync(Guid eventId, string? keyword, bool? isDisable, int pageIndex, int pageSize);
}
