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

    public async Task<CriteriaTemplates?> GetByIdAsync(Guid id)
        => await _context.Set<CriteriaTemplates>()
            .Include(ct => ct.CriteriaItems)
            .FirstOrDefaultAsync(ct => ct.Id == id);

    public async Task<List<CriteriaTemplates>> GetByRoundIdAsync(Guid roundId)
        => await _context.Set<CriteriaTemplates>()
            .Where(ct => ct.RoundId == roundId)
            .ToListAsync();

    public async Task<List<CriteriaItems>> GetItemsByTemplateIdAsync(Guid templateId)
        => await _context.Set<CriteriaItems>()
            .Where(ci => ci.CriteriaTemplateId == templateId)
            .ToListAsync();

    public async Task AddAsync(CriteriaTemplates template)
        => await _context.Set<CriteriaTemplates>().AddAsync(template);

    public Task UpdateAsync(CriteriaTemplates template)
    {
        _context.Set<CriteriaTemplates>().Update(template);
        return Task.CompletedTask;
    }
}
