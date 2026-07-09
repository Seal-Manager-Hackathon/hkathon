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

    public async Task<(List<Tracks> Items, int TotalCount)> SearchByEventIdAsync(Guid eventId, string? keyword, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Set<Tracks>()
            .Where(t => t.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        if (isDisable.HasValue)
            query = query.Where(t => t.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Tracks> Items, int TotalCount)> GetByEventIdAsync(Guid eventId, string? keyword, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Set<Tracks>()
            .Where(t => t.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        if (isDisable.HasValue)
            query = query.Where(t => t.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
