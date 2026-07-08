using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class RoundRepository : IRoundRepository
{
    private readonly AppDbContext _context;

    public RoundRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Rounds?> GetByIdAsync(Guid id)
        => await _context.Set<Rounds>().FindAsync(id);

    public async Task<Rounds?> GetDetailByIdAsync(Guid id)
        => await _context.Set<Rounds>()
            .Include(r => r.Event)
            .FirstOrDefaultAsync(r => r.Id == id);

    public Task UpdateAsync(Rounds round)
    {
        _context.Set<Rounds>().Update(round);
        return Task.CompletedTask;
    }

    public async Task<Rounds?> GetFirstRoundByEventIdAsync(Guid eventId)
        => await _context.Set<Rounds>()
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.RoundNo)
            .FirstOrDefaultAsync();

    public async Task<int> CountTeamsInRoundAsync(Guid roundId)
        => await _context.Set<RoundDetails>()
            .CountAsync(rd => rd.RoundId == roundId);

    public async Task AddRoundDetailAsync(RoundDetails roundDetail)
        => await _context.Set<RoundDetails>().AddAsync(roundDetail);

    public async Task<RoundDetails?> GetRoundDetailAsync(Guid registerTeamId, Guid roundId)
        => await _context.Set<RoundDetails>()
            .Where(rd => rd.RegisterTeamId == registerTeamId && rd.RoundId == roundId && !rd.IsDisable)
            .FirstOrDefaultAsync();

    public async Task<RoundDetails?> GetRoundDetailWithScoresAsync(Guid roundId, Guid registerTeamId)
        => await _context.Set<RoundDetails>()
            .Include(rd => rd.Round)
                .ThenInclude(r => r.Event)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Track)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Topic)
            .Include(rd => rd.Submissions)
                .ThenInclude(s => s.Scores)
                    .ThenInclude(sc => sc.ScoreItems)
                        .ThenInclude(si => si.CriteriaItem)
            .Include(rd => rd.Submissions)
                .ThenInclude(s => s.Scores)
                    .ThenInclude(sc => sc.AssignTrack)
                        .ThenInclude(at => at.Track)
            .Include(rd => rd.Submissions)
                .ThenInclude(s => s.Scores)
                    .ThenInclude(sc => sc.AssignTrack)
                        .ThenInclude(at => at.AssignEvent)
                            .ThenInclude(ae => ae.User)
            .Where(rd => rd.RoundId == roundId && rd.RegisterTeamId == registerTeamId && !rd.IsDisable)
            .FirstOrDefaultAsync();

    public Task RemoveRoundDetailAsync(RoundDetails roundDetail)
    {
        roundDetail.IsDisable = true;
        roundDetail.UpdatedAt = DateTimeOffset.UtcNow;
        _context.Set<RoundDetails>().Update(roundDetail);
        return Task.CompletedTask;
    }

    public async Task AddAsync(Rounds round)
        => await _context.Set<Rounds>().AddAsync(round);

    public async Task<Rounds?> GetByEventIdAndRoundNoAsync(Guid eventId, int roundNo)
        => await _context.Set<Rounds>()
            .Where(r => r.EventId == eventId && r.RoundNo == roundNo)
            .FirstOrDefaultAsync();

    public async Task<int?> GetMaxRoundNoAsync(Guid eventId)
        => await _context.Set<Rounds>()
            .Where(r => r.EventId == eventId)
            .MaxAsync(r => (int?)r.RoundNo);

    public async Task<(List<Rounds> Items, int TotalCount)> SearchByEventIdAsync(
        Guid eventId, string? keyword, int? roundNo, bool? isDisable,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<Rounds>()
            .Where(r => r.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(kw));
        }

        if (roundNo.HasValue)
            query = query.Where(r => r.RoundNo == roundNo.Value);

        if (isDisable.HasValue)
            query = query.Where(r => r.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(r => r.RoundNo)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Rounds>> GetRoundsGreaterThanRoundNoAsync(Guid eventId, int roundNo)
        => await _context.Set<Rounds>()
            .Where(r => r.EventId == eventId && r.RoundNo > roundNo)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
}
