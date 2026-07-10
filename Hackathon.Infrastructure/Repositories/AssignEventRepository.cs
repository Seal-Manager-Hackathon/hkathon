using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;
using Hackathon.Domain.Enums.EventRole;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class AssignEventRepository : IAssignEventRepository
{
    private readonly AppDbContext _context;

    public AssignEventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersByEventAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.User)
            .Include(ae => ae.EventRole)
            .Include(ae => ae.AssignTracks)
                .ThenInclude(at => at.Track)
            .Where(ae => ae.EventId == eventId);

        if (eventRole.HasValue)
            query = query.Where(ae => ae.EventRole != null && ae.EventRole.Name == eventRole.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(ae =>
                ae.User.Email.ToLower().Contains(kw) ||
                (ae.User.FirstName.ToLower() + " " + ae.User.LastName.ToLower()).Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(ae => ae.User.FirstName)
            .ThenBy(ae => ae.User.LastName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersByEventAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        Domain.Enums.User.RoleEnum? role, Guid? trackId,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.User)
            .Include(ae => ae.EventRole)
            .Include(ae => ae.AssignTracks)
                .ThenInclude(at => at.Track)
            .Where(ae => ae.EventId == eventId);

        if (eventRole.HasValue)
            query = query.Where(ae => ae.EventRole != null && ae.EventRole.Name == eventRole.Value);

        if (role.HasValue)
            query = query.Where(ae => ae.User.Role == role.Value);

        if (trackId.HasValue)
            query = query.Where(ae => ae.AssignTracks.Any(at => at.TrackId == trackId.Value && !at.IsDisable));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(ae =>
                ae.User.Email.ToLower().Contains(kw) ||
                (ae.User.FirstName.ToLower() + " " + ae.User.LastName.ToLower()).Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(ae => ae.User.FirstName)
            .ThenBy(ae => ae.User.LastName)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<AssignEvents?> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId)
        => await _context.AssignEvents
            .FirstOrDefaultAsync(ae => ae.EventId == eventId && ae.UserId == userId);

    public async Task<AssignEvents?> GetByIdAsync(Guid id)
        => await _context.AssignEvents
            .Include(ae => ae.User)
            .FirstOrDefaultAsync(ae => ae.Id == id);

    public void Add(AssignEvents assignEvent)
        => _context.AssignEvents.Add(assignEvent);

    public void Update(AssignEvents assignEvent)
        => _context.AssignEvents.Update(assignEvent);

    public async Task<AssignEvents?> GetByIdWithTracksAsync(Guid id)
        => await _context.AssignEvents
            .Include(ae => ae.User)
            .Include(ae => ae.EventRole)
            .Include(ae => ae.AssignTracks)
                .ThenInclude(at => at.Track)
            .FirstOrDefaultAsync(ae => ae.Id == id);

    public async Task<bool> IsTrackAssignedAsync(Guid assignEventId, Guid trackId)
        => await _context.AssignTracks
            .AnyAsync(at => at.AssignEventId == assignEventId && at.TrackId == trackId && !at.IsDisable);

    public void AddAssignTrack(AssignTracks assignTrack)
        => _context.AssignTracks.Add(assignTrack);

    public async Task<AssignTracks?> GetAssignTrackAsync(Guid assignEventId, Guid trackId)
        => await _context.AssignTracks
            .FirstOrDefaultAsync(at => at.AssignEventId == assignEventId && at.TrackId == trackId && !at.IsDisable);

    public async Task<AssignTracks?> GetAssignTrackAnyAsync(Guid assignEventId, Guid trackId)
        => await _context.AssignTracks
            .FirstOrDefaultAsync(at => at.AssignEventId == assignEventId && at.TrackId == trackId);

    public async Task<AssignTracks?> GetGraderAssignTrackAsync(Guid userId, Guid eventId, Guid trackId)
        => await _context.AssignTracks
            .Include(at => at.AssignEvent)
                .ThenInclude(ae => ae.EventRole)
            .Where(at => !at.IsDisable
                && at.AssignEvent.UserId == userId
                && at.AssignEvent.EventId == eventId
                && at.AssignEvent.EventRole!.Name == Domain.Enums.EventRole.EventRoleEnum.Judge
                && at.TrackId == trackId)
            .FirstOrDefaultAsync();

    public void RemoveAssignTrack(AssignTracks assignTrack)
        => _context.AssignTracks.Update(assignTrack);

    public void RestoreAssignTrack(AssignTracks assignTrack)
        => _context.AssignTracks.Update(assignTrack);

    public async Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName)
        => await _context.Set<EventRoles>()
            .FirstOrDefaultAsync(er => er.Name == roleName);

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetEventsByStaffUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .Where(ae => ae.UserId == userId
                && ae.Event.Status != EventStatusEnum.Draft);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(ae => ae.Event.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(ae => ae.Event.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(ae => ae.Event.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetStaffAssignEventsByUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .Where(ae => ae.UserId == userId
                && !ae.IsDisable
                && ae.EventRole != null
                && ae.EventRole.Name == EventRoleEnum.Staff
                && ae.Event.Status != EventStatusEnum.Draft);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(ae => ae.Event.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(ae => ae.Event.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(ae => ae.Event.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<AssignEvents>> GetCurrentAssignedEventsByUserIdAsync(Guid userId)
    {
        var now = DateTimeOffset.UtcNow;

        return await _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .Where(ae => ae.UserId == userId
                && !ae.IsDisable
                && ae.EventRole != null
                && ae.EventRole.Name == EventRoleEnum.Staff
                && ae.Event.Status != EventStatusEnum.Draft
                && ae.Event.StartTime.HasValue
                && ae.Event.EndTime.HasValue
                && ae.Event.StartTime <= now
                && ae.Event.EndTime >= now)
            .OrderByDescending(ae => ae.Event.StartTime)
            .ToListAsync();
    }

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetLecturerAssignEventsByUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .Where(ae => ae.UserId == userId
                && ae.EventRole != null
                && (ae.EventRole.Name == EventRoleEnum.Judge || ae.EventRole.Name == EventRoleEnum.Mentor)
                && ae.Event.Status != EventStatusEnum.Draft);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(ae => ae.Event.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(ae => ae.Event.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(ae => ae.Event.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(ae => ae.Event.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<AssignEvents?> GetByEventIdAndUserIdWithTracksAsync(Guid eventId, Guid userId)
        => await _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .Include(ae => ae.AssignTracks)
                .ThenInclude(at => at.Track)
            .FirstOrDefaultAsync(ae => ae.EventId == eventId
                && ae.UserId == userId
                && !ae.IsDisable
                && !ae.Event.IsDisable);

    public async Task<AssignEvents?> GetByEventIdAndUserIdWithEventAsync(Guid eventId, Guid userId)
        => await _context.AssignEvents
            .Include(ae => ae.Event)
            .Include(ae => ae.EventRole)
            .FirstOrDefaultAsync(ae => ae.EventId == eventId
                && ae.UserId == userId
                && !ae.Event.IsDisable);
}
