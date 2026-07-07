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

    public async Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersAsync(
        Guid eventId, string? keyword, Domain.Enums.User.RoleEnum? role,
        int pageIndex, int pageSize)
    {
        var query = _context.AssignEvents
            .Include(ae => ae.User)
            .Include(ae => ae.EventRole)
            .Include(ae => ae.AssignTracks)
                .ThenInclude(at => at.Track)
            .Where(ae => ae.EventId == eventId);

        if (role.HasValue)
            query = query.Where(ae => ae.User.Role == role.Value);

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

    public void Add(AssignEvents assignEvent)
        => _context.AssignEvents.Add(assignEvent);

    public async Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName)
        => await _context.Set<EventRoles>()
            .FirstOrDefaultAsync(er => er.Name == roleName);
}
