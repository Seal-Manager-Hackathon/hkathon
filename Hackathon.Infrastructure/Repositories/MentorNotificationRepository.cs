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
        => await _context.MentorNotifications
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.Event)
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.EventRole)
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.Track)
            .FirstOrDefaultAsync(mn => mn.Id == id);

    public async Task<List<MentorNotifications>> GetByAssignTrackIdAsync(Guid assignTrackId, int pageIndex, int pageSize)
    {
        var query = _context.MentorNotifications
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.Track)
            .Where(mn => mn.AssignTrackId == assignTrackId && !mn.IsDisable);

        return await query
            .OrderByDescending(mn => mn.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<(List<MentorNotifications> Items, int TotalCount)> GetByAssignTrackIdsAsync(List<Guid> assignTrackIds, int pageIndex, int pageSize)
    {
        var query = _context.MentorNotifications
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(mn => mn.AssignTrack)
                .ThenInclude(at => at.Track)
            .Where(mn => assignTrackIds.Contains(mn.AssignTrackId) && !mn.IsDisable);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(mn => mn.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
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
