using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountAsync(bool? isDisable)
    {
        var query = _context.Set<Teams>().AsQueryable();

        if (isDisable.HasValue)
            query = query.Where(t => t.IsDisable == isDisable.Value);

        return await query.CountAsync();
    }

    public async Task<Teams?> GetByIdAsync(Guid id)
        => await _context.Set<Teams>().FindAsync(id);

    public async Task<List<Guid>> GetTeamMemberIdsAsync(Guid teamId)
        => await _context.Set<TeamDetails>()
            .Where(td => td.TeamId == teamId)
            .Select(td => td.UserId)
            .ToListAsync();

    public async Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId)
        => await _context.Set<TeamDetails>().AnyAsync(td => td.TeamId == teamId && td.UserId == userId);

    public async Task<List<TeamDetails>> GetTeamMembersAsync(Guid teamId)
        => await _context.Set<TeamDetails>()
            .Include(td => td.User)
            .Where(td => td.TeamId == teamId)
            .ToListAsync();

    public Task UpdateAsync(Teams team)
    {
        _context.Set<Teams>().Update(team);
        return Task.CompletedTask;
    }

    public async Task<(List<Teams> Items, int TotalCount)> SearchAsync(
        string? keyword, bool? canEdit,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<Teams>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(kw));
        }

        if (canEdit.HasValue)
            query = query.Where(t => t.CanEdit == canEdit.Value);

        if (fromDate.HasValue)
            query = query.Where(t => t.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(t => t.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
