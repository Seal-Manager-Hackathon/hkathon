using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class MentorNotificationRepository : IMentorNotificationRepository
{
    private readonly AppDbContext _context;

    public MentorNotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MentorNotifications?> GetByIdAsync(Guid id)
        => await _context.MentorNotifications.FindAsync(id);

    public async Task<List<MentorNotifications>> GetByAssignTrackIdAsync(Guid assignTrackId, int pageIndex, int pageSize)
    {
        var query = _context.MentorNotifications
            .Where(mn => mn.AssignTrackId == assignTrackId && !mn.IsDisable);

        return await query
            .OrderByDescending(mn => mn.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountByAssignTrackIdAsync(Guid assignTrackId)
        => await _context.MentorNotifications
            .CountAsync(mn => mn.AssignTrackId == assignTrackId && !mn.IsDisable);

    public async Task AddAsync(MentorNotifications notification)
        => await _context.MentorNotifications.AddAsync(notification);

    public Task UpdateAsync(MentorNotifications notification)
    {
        _context.MentorNotifications.Update(notification);
        return Task.CompletedTask;
    }
}
