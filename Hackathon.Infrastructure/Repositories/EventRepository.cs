using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
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

    public async Task<List<Events>> GetRecentAsync(int count)
        => await _context.Events
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<int> CountByStatusAsync(EventStatusEnum? status)
    {
        var query = _context.Events.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(e => e.Status == status.Value);
        }

        return await query.CountAsync();
    }
}
