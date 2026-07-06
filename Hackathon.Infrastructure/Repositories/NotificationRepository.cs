using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Notifications?> GetByIdAsync(Guid id)
        => await _context.Notifications.FindAsync(id);

    public async Task AddAsync(Notifications notification)
        => await _context.Notifications.AddAsync(notification);

    public async Task AddRangeAsync(List<Notifications> notifications)
        => await _context.Notifications.AddRangeAsync(notifications);

    public Task UpdateAsync(Notifications notification)
    {
        _context.Notifications.Update(notification);
        return Task.CompletedTask;
    }

    public async Task<List<Notifications>> GetRecentAsync(int count)
        => await _context.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<(List<Notifications> Items, int TotalCount)> SearchAsync(
        string? title, NotificationTargetTypeEnum? targetType,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Notifications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            var kw = title.Trim().ToLower();
            query = query.Where(n => n.Title != null && n.Title.ToLower().Contains(kw));
        }

        if (targetType.HasValue)
            query = query.Where(n => n.TargetType == targetType.Value);

        if (fromDate.HasValue)
            query = query.Where(n => n.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(n => n.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
