using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.RegisterTeam;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Events>> GetAllAsync()
        => await _context.Events.ToListAsync();

    public async Task<Events?> GetByIdAsync(Guid id)
        => await _context.Events.FindAsync(id);

    public async Task AddAsync(Events ev)
        => await _context.Events.AddAsync(ev);

    public Task UpdateAsync(Events ev)
    {
        _context.Events.Update(ev);
        return Task.CompletedTask;
    }

    public async Task AddLeaderBoardAsync(LeaderBoards leaderBoard)
        => await _context.Set<LeaderBoards>().AddAsync(leaderBoard);

    public async Task<LeaderBoards?> GetLeaderBoardByEventIdAsync(Guid eventId)
        => await _context.Set<LeaderBoards>()
            .FirstOrDefaultAsync(lb => lb.EventId == eventId);

    public Task UpdateLeaderBoardAsync(LeaderBoards leaderBoard)
    {
        _context.Set<LeaderBoards>().Update(leaderBoard);
        return Task.CompletedTask;
    }

    public async Task<List<Events>> GetPublishedByYearAsync(int year)
        => await _context.Events
            .Include(e => e.RegisterTeams)
                .ThenInclude(rt => rt.Team)
            .Include(e => e.RegisterTeams)
                .ThenInclude(rt => rt.Track)
            .Include(e => e.RegisterTeams)
                .ThenInclude(rt => rt.Topic)
            .Include(e => e.RegisterTeams)
                .ThenInclude(rt => rt.RoundDetails)
                    .ThenInclude(rd => rd.Round)
            .Include(e => e.RegisterTeams)
                .ThenInclude(rt => rt.RoundDetails)
                    .ThenInclude(rd => rd.Submissions)
                        .ThenInclude(s => s.Scores)
                            .ThenInclude(sc => sc.ScoreItems)
            .Where(e => e.Status == EventStatusEnum.Published
                && e.CreatedAt.Year == year
                && !e.IsDisable)
            .ToListAsync();

    public async Task<List<LeaderBoards>> GetLeaderBoardByYearAsync(int year)
        => await _context.Set<LeaderBoards>()
            .Include(lb => lb.Event)
                .ThenInclude(ev => ev.RegisterTeams)
                    .ThenInclude(rt => rt.Team)
            .Include(lb => lb.Event)
                .ThenInclude(ev => ev.RegisterTeams)
                    .ThenInclude(rt => rt.Track)
            .Include(lb => lb.Event)
                .ThenInclude(ev => ev.RegisterTeams)
                    .ThenInclude(rt => rt.Topic)
            .Include(lb => lb.Event)
                .ThenInclude(ev => ev.RegisterTeams)
                    .ThenInclude(rt => rt.RoundDetails)
                        .ThenInclude(rd => rd.Round)
            .Include(lb => lb.Event)
                .ThenInclude(ev => ev.RegisterTeams)
                    .ThenInclude(rt => rt.RoundDetails)
                        .ThenInclude(rd => rd.Submissions)
                            .ThenInclude(s => s.Scores)
                                .ThenInclude(sc => sc.ScoreItems)
            .Where(lb => lb.Year == year
                && lb.Event != null
                && !lb.Event.IsDisable
                && (lb.Event.Status == EventStatusEnum.Published || lb.Event.Status == EventStatusEnum.Closed))
            .ToListAsync();

    public async Task<List<Events>> GetRecentAsync(int count)
        => await _context.Events
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<int> CountByStatusAsync(EventStatusEnum? status)
    {
        var query = _context.Events.AsQueryable();

        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        return await query.CountAsync();
    }

    public async Task<(List<Events> Items, int TotalCount)> SearchAsync(
        string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate, bool? isDisable,
        int pageIndex, int pageSize)
    {
        var query = _context.Events.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(e => e.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(e => e.CreatedAt <= toDate.Value);

        if (isDisable.HasValue)
            query = query.Where(e => e.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Events> Items, int TotalCount)> GetPublicEventsAsync(int pageIndex, int pageSize)
    {
        var query = _context.Events
            .Include(e => e.RegisterTeams)
            .Where(e => e.Status == EventStatusEnum.Published && !e.IsDisable)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(e => e.RegisterTeams.Count(rt => rt.Status == RegisterTeamStatusEnum.Approved && !rt.IsDisable))
            .ThenByDescending(e => e.RegisterTeams.Count)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
