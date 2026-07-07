using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class ScoreRepository : IScoreRepository
{
    private readonly AppDbContext _context;

    public ScoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Scores>> GetBySubmissionIdAsync(Guid submissionId)
    {
        return await _context.Set<Scores>()
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.Track)
            .Include(s => s.ScoreItems)
                .ThenInclude(si => si.CriteriaItem)
            .Where(s => s.SubmissionId == submissionId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<(List<ScoreItems> Items, int TotalCount)> GetScoreItemsByScoreIdAsync(Guid scoreId, int pageIndex, int pageSize)
    {
        var query = _context.Set<ScoreItems>()
            .Include(si => si.CriteriaItem)
            .Where(si => si.ScoreId == scoreId)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(si => si.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
