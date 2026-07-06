using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class RegisterTeamRepository : IRegisterTeamRepository
{
    private readonly AppDbContext _context;

    public RegisterTeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterTeams?> GetByIdAsync(Guid id)
        => await _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .FirstOrDefaultAsync(rt => rt.Id == id);

    public Task UpdateAsync(RegisterTeams registerTeam)
    {
        _context.Set<RegisterTeams>().Update(registerTeam);
        return Task.CompletedTask;
    }

    public async Task<bool> HasOtherApprovedAsync(Guid teamId, Guid excludeRegisterTeamId)
        => await _context.Set<RegisterTeams>()
            .AnyAsync(rt => rt.TeamId == teamId
                && rt.Id != excludeRegisterTeamId
                && rt.Status == RegisterTeamStatusEnum.Approved);

    public async Task<(List<RegisterTeams> Items, int TotalCount)> SearchAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Where(rt => rt.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rt => rt.Team.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(rt => rt.Status == status.Value);

        if (isBanned.HasValue)
            query = query.Where(rt => rt.IsBanned == isBanned.Value);

        if (isDisable.HasValue)
            query = query.Where(rt => rt.IsDisable == isDisable.Value);

        if (fromDate.HasValue)
            query = query.Where(rt => rt.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(rt => rt.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
