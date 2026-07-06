using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reports>> GetRecentAsync(int count)
        => await _context.Set<Reports>()
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .ToListAsync();
}
