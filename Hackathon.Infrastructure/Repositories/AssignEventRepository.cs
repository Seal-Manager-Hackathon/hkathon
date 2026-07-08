using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
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
            .FirstOrDefaultAsync(ae => ae.Id == id);

    public async Task<bool> IsTrackAssignedAsync(Guid assignEventId, Guid trackId)
        => await _context.AssignTracks
            .AnyAsync(at => at.AssignEventId == assignEventId && at.TrackId == trackId && !at.IsDisable);

    public void AddAssignTrack(AssignTracks assignTrack)
        => _context.AssignTracks.Add(assignTrack);

    public async Task<AssignTracks?> GetAssignTrackAsync(Guid assignEventId, Guid trackId)
        => await _context.AssignTracks
            .FirstOrDefaultAsync(at => at.AssignEventId == assignEventId && at.TrackId == trackId && !at.IsDisable);

    public void RemoveAssignTrack(AssignTracks assignTrack)
        => _context.AssignTracks.Update(assignTrack);

    public async Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName)
        => await _context.Set<EventRoles>()
            .FirstOrDefaultAsync(er => er.Name == roleName);

}
