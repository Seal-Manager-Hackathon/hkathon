using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;

namespace Hackathon.Application.Common.IRepository;

public interface IEventRepository
{
    Task<List<Events>> GetAllAsync();
    Task<int> CountByStatusAsync(EventStatusEnum? status);
    Task<List<Events>> GetRecentAsync(int count);
}
