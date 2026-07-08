using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly AppDbContext _context;

    public SubmissionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Submissions?> GetByIdAsync(Guid submissionId)
        => await _context.Set<Submissions>()
            .Include(s => s.RoundDetail)
                .ThenInclude(rd => rd.Round)
            .Include(s => s.RoundDetail)
                .ThenInclude(rd => rd.RegisterTeam)
                    .ThenInclude(rt => rt.Team)
                        .ThenInclude(t => t.TeamDetails)
                            .ThenInclude(td => td.User)
            .Include(s => s.RoundDetail)
                .ThenInclude(rd => rd.RegisterTeam)
                    .ThenInclude(rt => rt.Event)
            .Include(s => s.RoundDetail)
                .ThenInclude(rd => rd.RegisterTeam)
                    .ThenInclude(rt => rt.Track)
            .Include(s => s.RoundDetail)
                .ThenInclude(rd => rd.RegisterTeam)
                    .ThenInclude(rt => rt.Topic)
            .Include(s => s.Scores)
                .ThenInclude(sc => sc.AssignTrack)
                    .ThenInclude(at => at.Track)
            .Include(s => s.Scores)
                .ThenInclude(sc => sc.ScoreItems)
                    .ThenInclude(si => si.CriteriaItem)
            .Include(s => s.Scores)
                .ThenInclude(sc => sc.ScoreItems)
                    .ThenInclude(si => si.AssignTrack)
                        .ThenInclude(at => at.AssignEvent)
                            .ThenInclude(ae => ae.User)
            .FirstOrDefaultAsync(s => s.Id == submissionId);

    public async Task<(List<RoundDetails> Items, int TotalCount)> GetSubmissionsAsync(
        Guid eventId, Guid? roundId, Guid? trackId, Guid? topicId, Guid? registerTeamId, string? keyword,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<RoundDetails>()
            .Include(rd => rd.Round)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Team)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Event)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Track)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Topic)
            .Include(rd => rd.Submissions)
            .Include(rd => rd.RegisterTeam.Team.TeamDetails)
                .ThenInclude(td => td.User)
            .Where(rd => rd.Round.EventId == eventId)
            .AsQueryable();

        if (roundId.HasValue)
            query = query.Where(rd => rd.RoundId == roundId.Value);
        if (trackId.HasValue)
            query = query.Where(rd => rd.RegisterTeam.TrackId == trackId.Value);
        if (topicId.HasValue)
            query = query.Where(rd => rd.RegisterTeam.TopicId == topicId.Value);
        if (registerTeamId.HasValue)
            query = query.Where(rd => rd.RegisterTeamId == registerTeamId.Value);
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rd => rd.RegisterTeam.Team.Name.ToLower().Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rd => rd.Submissions
                .Select(s => (DateTimeOffset?)s.SubmittedAt)
                .Max() ?? DateTimeOffset.MinValue)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<RoundSummaryItem> Items, int TotalCount)> GetRoundSummaryAsync(
        Guid roundId, int pageIndex, int pageSize)
    {
        // Lấy tất cả register teams có trong round này (qua RoundDetails)
        var query = _context.Set<RoundDetails>()
            .Include(rd => rd.Round)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Team)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Track)
            .Include(rd => rd.RegisterTeam)
                .ThenInclude(rt => rt.Topic)
            .Include(rd => rd.Submissions)
                .ThenInclude(s => s.Scores)
                    .ThenInclude(sc => sc.ScoreItems)
            .Where(rd => rd.RoundId == roundId)
            .AsQueryable();

        var totalCount = await query.CountAsync();

        var roundDetails = await query
            .OrderByDescending(rd => rd.Submissions
                .OrderByDescending(s => s.SubmittedAt)
                .SelectMany(s => s.Scores)
                .Sum(sc => (decimal?)sc.TotalScore ?? 0))
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = roundDetails.Select(rd =>
        {
            var lastSubmission = SubmissionHelper.GetLastSubmission(rd);

            // scopeScore = SUM(Scores.TotalScore)
            var scopeScore = lastSubmission?.Scores
                .Where(s => s.TotalScore.HasValue)
                .Sum(s => s.TotalScore!.Value) ?? 0m;

            return new RoundSummaryItem
            {
                RoundId = rd.RoundId,
                EventId = rd.Round.EventId,
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                LastSubmissionId = lastSubmission?.Id,
                TotalScore = Math.Round(scopeScore, 2),
                RoundNo = rd.Round.RoundNo
            };
        }).ToList();

        return (items, totalCount);
    }
}
