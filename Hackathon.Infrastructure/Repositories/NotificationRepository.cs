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

    public async Task<Notifications?> GetDetailByIdAsync(Guid id)
        => await _context.Notifications
            .Include(n => n.User)
            .Include(n => n.Team)
            .FirstOrDefaultAsync(n => n.Id == id);

    public async Task AddAsync(Notifications notification)
        => await _context.Notifications.AddAsync(notification);

    public async Task AddRangeAsync(List<Notifications> notifications)
        => await _context.Notifications.AddRangeAsync(notifications);

    public Task UpdateAsync(Notifications notification)
    {
        _context.Notifications.Update(notification);
        return Task.CompletedTask;
    }

    public Task UpdateRangeAsync(List<Notifications> notifications)
    {
        _context.Notifications.UpdateRange(notifications);
        return Task.CompletedTask;
    }

    public async Task<List<Notifications>> GetRecentAsync(int count)
        => await _context.Notifications
            .Where(n => !n.IsDisable)
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<(List<Notifications> Items, int TotalCount)> GetUserNotificationsAsync(
        Guid userId, string? keyword, NotificationTargetTypeEnum? targetType,
        NotificationStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Notifications
            .Where(n => !n.IsDisable
                && (n.UserId == userId
                    || n.TargetType == NotificationTargetTypeEnum.System
                    || n.TargetType == NotificationTargetTypeEnum.Team));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(n => n.Title != null && n.Title.ToLower().Contains(kw));
        }

        if (targetType.HasValue)
            query = query.Where(n => n.TargetType == targetType.Value);

        if (status.HasValue)
            query = query.Where(n => n.Status == status.Value);

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

    public async Task<int> CountStudentNotificationsAsync(
        Guid userId, List<Guid> teamIds, string? keyword,
        NotificationTargetTypeEnum? targetType,
        NotificationStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate)
    {
        var query = _context.Notifications
            .Where(n => !n.IsDisable
                && (n.UserId == userId
                    || n.TargetType == NotificationTargetTypeEnum.System
                    || (n.TargetType == NotificationTargetTypeEnum.Team
                        && n.TeamId.HasValue
                        && teamIds.Contains(n.TeamId.Value))));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(n => n.Title != null && n.Title.ToLower().Contains(kw));
        }

        if (targetType.HasValue)
            query = query.Where(n => n.TargetType == targetType.Value);

        if (status.HasValue)
            query = query.Where(n => n.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(n => n.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(n => n.CreatedAt <= toDate.Value);

        return await query.CountAsync();
    }

    public async Task<(List<Notifications> Items, int TotalCount)> GetStudentNotificationsAsync(
        Guid userId, List<Guid> teamIds, string? keyword,
        NotificationTargetTypeEnum? targetType,
        NotificationStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Notifications
            .Where(n => !n.IsDisable
                && (n.UserId == userId
                    || n.TargetType == NotificationTargetTypeEnum.System
                    || (n.TargetType == NotificationTargetTypeEnum.Team
                        && n.TeamId.HasValue
                        && teamIds.Contains(n.TeamId.Value))));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(n => n.Title != null && n.Title.ToLower().Contains(kw));
        }

        if (targetType.HasValue)
            query = query.Where(n => n.TargetType == targetType.Value);

        if (status.HasValue)
            query = query.Where(n => n.Status == status.Value);

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

    public async Task<(List<Notifications> Items, int TotalCount)> SearchAsync(
        string? title, NotificationTargetTypeEnum? targetType,
        DateTimeOffset? fromDate, DateTimeOffset? toDate, bool? isDisable,
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

        if (isDisable.HasValue)
            query = query.Where(n => n.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
