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

    public async Task<(List<Topics> Items, int TotalCount)> SearchByTrackIdAsync(Guid trackId, string? keyword, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Set<Topics>()
            .Include(t => t.Track)
            .Where(t => t.TrackId == trackId)
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

    public async Task<(List<Topics> Items, int TotalCount)> SearchAsync(Guid trackId, string? keyword, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Set<Topics>()
            .Include(t => t.Track)
            .Where(t => t.TrackId == trackId)
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
