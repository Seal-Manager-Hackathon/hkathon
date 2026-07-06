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
}
