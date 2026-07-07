using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class CriteriaItemRepository : ICriteriaItemRepository
{
    private readonly AppDbContext _context;

    public CriteriaItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CriteriaItems?> GetByIdAsync(Guid id)
        => await _context.Set<CriteriaItems>()
            .FirstOrDefaultAsync(ci => ci.Id == id);
}