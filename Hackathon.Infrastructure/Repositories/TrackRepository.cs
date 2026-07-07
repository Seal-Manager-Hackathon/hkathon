using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly AppDbContext _context;

    public TrackRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tracks?> GetByIdAsync(Guid id)
        => await _context.Set<Tracks>().FindAsync(id);

    public async Task<List<Tracks>> GetByEventIdAsync(Guid eventId)
        => await _context.Set<Tracks>()
            .Where(t => t.EventId == eventId)
            .ToListAsync();

    public async Task AddAsync(Tracks track)
        => await _context.Set<Tracks>().AddAsync(track);
}
