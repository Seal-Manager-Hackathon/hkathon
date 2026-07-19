using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly AppDbContext _context;

    public InvitationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<Invitations> Items, int TotalCount)> GetByTeamIdAsync(
        Guid teamId, string? keyword, InvitationStatusEnum? status,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<Invitations>()
            .Include(i => i.User)
            .Include(i => i.Team)
            .Where(i => i.TeamId == teamId)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(i => i.User.Email.ToLower().Contains(kw)
                || (i.User.FirstName + " " + i.User.LastName).ToLower().Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Invitations invitation)
        => await _context.Set<Invitations>().AddAsync(invitation);

    public async Task<Invitations?> GetByIdAsync(Guid id)
        => await _context.Set<Invitations>()
            .Include(i => i.User)
            .Include(i => i.Team)
                .ThenInclude(t => t!.TeamDetails)
                .ThenInclude(td => td.User)
            .FirstOrDefaultAsync(i => i.Id == id);

    public async Task<(List<Invitations> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, string? keyword, InvitationStatusEnum? status,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<Invitations>()
            .Include(i => i.Team)
                .ThenInclude(t => t!.TeamDetails)
                .ThenInclude(td => td.User)
            .Include(i => i.User)
            .Where(i => i.UserId == userId)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(i => i.Team.Name.ToLower().Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(i => i.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
