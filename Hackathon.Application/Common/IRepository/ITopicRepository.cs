using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITopicRepository
{
    Task<Topics?> GetByIdAsync(Guid id);
    Task<List<Topics>> GetByTrackIdAsync(Guid trackId);
    Task AddAsync(Topics topic);
}
