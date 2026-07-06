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
}
