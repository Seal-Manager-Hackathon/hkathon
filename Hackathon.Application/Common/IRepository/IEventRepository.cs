using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;

namespace Hackathon.Application.Common.IRepository;

public interface IEventRepository
{
    Task<List<Events>> GetAllAsync();
    Task<Events?> GetByIdAsync(Guid id);
    Task AddAsync(Events ev);
    Task UpdateAsync(Events ev);
    Task<int> CountByStatusAsync(EventStatusEnum? status);
    Task AddLeaderBoardAsync(LeaderBoards leaderBoard);
    Task<LeaderBoards?> GetLeaderBoardByEventIdAsync(Guid eventId);
    Task UpdateLeaderBoardAsync(LeaderBoards leaderBoard);
    Task<List<Events>> GetRecentAsync(int count);
    Task<List<Events>> GetPublishedByYearAsync(int year);
    Task<(List<Events> Items, int TotalCount)> SearchAsync(
        string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate, bool? isDisable,
        int pageIndex, int pageSize);
}
