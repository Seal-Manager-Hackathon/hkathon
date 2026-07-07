using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class CriteriaTemplateRepository : ICriteriaTemplateRepository
{
    private readonly AppDbContext _context;

    public CriteriaTemplateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CriteriaTemplates>> GetByRoundIdAsync(Guid roundId)
        => await _context.Set<CriteriaTemplates>()
            .Include(ct => ct.CriteriaItems)
            .Where(ct => ct.RoundId == roundId)
            .ToListAsync();
}
