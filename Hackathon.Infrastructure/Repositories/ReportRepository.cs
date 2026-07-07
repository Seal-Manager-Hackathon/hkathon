using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Report;
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
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<Reports?> GetByIdAsync(Guid reportId)
        => await _context.Set<Reports>()
            .Include(r => r.User)
            .Include(r => r.AssignEvent)
                .ThenInclude(ae => ae.User)
            .Include(r => r.Submission)
            .FirstOrDefaultAsync(r => r.Id == reportId);

    public async Task<(List<Reports> Items, int TotalCount)> SearchAsync(
        string? keyword, ReportStatusEnum? status,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<Reports>()
            .Include(r => r.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(r =>
                (r.User.Email != null && r.User.Email.ToLower().Contains(kw)) ||
                ((r.User.FirstName + " " + r.User.LastName).ToLower().Contains(kw)) ||
                (r.Title != null && r.Title.ToLower().Contains(kw))
            );
        }

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
