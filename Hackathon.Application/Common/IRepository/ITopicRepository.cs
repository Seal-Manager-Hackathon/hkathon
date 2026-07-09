using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITopicRepository
{
    Task<Topics?> GetByIdAsync(Guid id);
    Task<List<Topics>> GetByTrackIdAsync(Guid trackId);
    Task AddAsync(Topics topic);
    Task<(List<Topics> Items, int TotalCount)> SearchByTrackIdAsync(Guid trackId, string? keyword, bool? isDisable, int pageIndex, int pageSize);
    Task<(List<Topics> Items, int TotalCount)> SearchAsync(Guid trackId, string? keyword, bool? isDisable, int pageIndex, int pageSize);
}
