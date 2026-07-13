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

    public async Task<Scores?> GetByIdAsync(Guid scoreId)
        => await _context.Set<Scores>()
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.Track)
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Track)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Topic)
            .Include(s => s.ScoreItems)
                .ThenInclude(si => si.CriteriaItem)
            .Include(s => s.ScoreItems)
                .ThenInclude(si => si.AssignTrack)
                    .ThenInclude(at => at.AssignEvent)
                        .ThenInclude(ae => ae.User)
            .FirstOrDefaultAsync(s => s.Id == scoreId);

    public async Task<List<Scores>> GetBySubmissionIdAsync(Guid submissionId)
    {
        return await _context.Set<Scores>()
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.Track)
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Track)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Topic)
            .Include(s => s.ScoreItems)
                .ThenInclude(si => si.CriteriaItem)
            .Include(s => s.ScoreItems)
                .ThenInclude(si => si.AssignTrack)
                    .ThenInclude(at => at.AssignEvent)
                        .ThenInclude(ae => ae.User)
            .Where(s => s.SubmissionId == submissionId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Scores>> GetByAssignTrackIdAsync(Guid assignTrackId)
    {
        return await _context.Set<Scores>()
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.Round)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Team)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Track)
            .Where(s => s.AssignTrackId == assignTrackId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Scores score)
        => await _context.Set<Scores>().AddAsync(score);

    public Task UpdateAsync(Scores score)
    {
        _context.Set<Scores>().Update(score);
        return Task.CompletedTask;
    }

    public async Task DeleteScoreItemsByScoreIdAsync(Guid scoreId)
    {
        var items = await _context.Set<ScoreItems>()
            .Where(si => si.ScoreId == scoreId)
            .ToListAsync();
        _context.Set<ScoreItems>().RemoveRange(items);
    }

    public async Task ReplaceScoreItemsAsync(Guid scoreId, List<ScoreItems> newItems)
    {
        var oldItems = await _context.Set<ScoreItems>()
            .Where(si => si.ScoreId == scoreId)
            .ToListAsync();
        _context.Set<ScoreItems>().RemoveRange(oldItems);
        await _context.Set<ScoreItems>().AddRangeAsync(newItems);
    }

    public async Task<(List<Scores> Items, int TotalCount)> GetScoresBySubmissionIdAsync(Guid submissionId, int pageIndex, int pageSize)
    {
        var query = _context.Set<Scores>()
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.Track)
            .Include(s => s.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Track)
            .Include(s => s.Submission)
                .ThenInclude(sub => sub.RoundDetail)
                    .ThenInclude(rd => rd.RegisterTeam)
                        .ThenInclude(rt => rt.Topic)
            .Where(s => s.SubmissionId == submissionId)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(s => s.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<ScoreItems?> GetScoreItemByIdAsync(Guid scoreItemId)
        => await _context.Set<ScoreItems>()
            .Include(si => si.CriteriaItem)
            .Include(si => si.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(si => si.ScoreEntity)
                .ThenInclude(s => s.Submission)
                    .ThenInclude(sub => sub.RoundDetail)
                        .ThenInclude(rd => rd.RegisterTeam)
                            .ThenInclude(rt => rt.Track)
            .Include(si => si.ScoreEntity)
                .ThenInclude(s => s.Submission)
                    .ThenInclude(sub => sub.RoundDetail)
                        .ThenInclude(rd => rd.RegisterTeam)
                            .ThenInclude(rt => rt.Topic)
            .FirstOrDefaultAsync(si => si.Id == scoreItemId);

    public async Task<(List<ScoreItems> Items, int TotalCount)> GetScoreItemsByScoreIdAsync(Guid scoreId, int pageIndex, int pageSize)
    {
        var query = _context.Set<ScoreItems>()
            .Include(si => si.CriteriaItem)
            .Include(si => si.AssignTrack)
                .ThenInclude(at => at.AssignEvent)
                    .ThenInclude(ae => ae.User)
            .Include(si => si.ScoreEntity)
                .ThenInclude(s => s.Submission)
                    .ThenInclude(sub => sub.RoundDetail)
                        .ThenInclude(rd => rd.RegisterTeam)
                            .ThenInclude(rt => rt.Track)
            .Include(si => si.ScoreEntity)
                .ThenInclude(s => s.Submission)
                    .ThenInclude(sub => sub.RoundDetail)
                        .ThenInclude(rd => rd.RegisterTeam)
                            .ThenInclude(rt => rt.Topic)
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
