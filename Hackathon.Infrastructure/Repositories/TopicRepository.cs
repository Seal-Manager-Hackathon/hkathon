using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly AppDbContext _context;

    public TopicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Topics?> GetByIdAsync(Guid id)
        => await _context.Set<Topics>()
            .Include(t => t.Track)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<List<Topics>> GetByTrackIdAsync(Guid trackId)
        => await _context.Set<Topics>()
            .Include(t => t.Track)
            .Where(t => t.TrackId == trackId)
            .ToListAsync();

    public async Task AddAsync(Topics topic)
        => await _context.Set<Topics>().AddAsync(topic);
}
