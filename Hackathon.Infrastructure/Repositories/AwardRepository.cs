using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class AwardRepository : IAwardRepository
{
    private readonly AppDbContext _context;

    public AwardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Awards>> GetByEventIdAsync(Guid eventId)
        => await _context.Set<Awards>()
            .Where(a => a.EventId == eventId)
            .ToListAsync();

    public async Task<Awards?> GetByIdAsync(Guid id)
        => await _context.Set<Awards>().FindAsync(id);

    public async Task AddAsync(Awards award)
        => await _context.Set<Awards>().AddAsync(award);

    public Task UpdateAsync(Awards award)
    {
        _context.Set<Awards>().Update(award);
        return Task.CompletedTask;
    }
}
