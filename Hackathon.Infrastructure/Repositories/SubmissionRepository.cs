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
            .OrderBy(rd => rd.Round.RoundNo)
            .ThenBy(rd => rd.RegisterTeam.Team.Name)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
