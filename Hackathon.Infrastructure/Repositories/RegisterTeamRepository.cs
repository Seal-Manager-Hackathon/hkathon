using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class RegisterTeamRepository : IRegisterTeamRepository
{
    private readonly AppDbContext _context;

    public RegisterTeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterTeams?> GetByIdAsync(Guid id)
        => await _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .FirstOrDefaultAsync(rt => rt.Id == id);

    public async Task AddAsync(RegisterTeams registerTeam)
        => await _context.Set<RegisterTeams>().AddAsync(registerTeam);

    public async Task<bool> HasAnyMemberApprovedInEventAsync(Guid eventId, List<Guid> userIds)
        => await _context.Set<RegisterTeams>()
            .AnyAsync(rt => rt.EventId == eventId
                && rt.Status == RegisterTeamStatusEnum.Approved
                && rt.Team.TeamDetails.Any(td => userIds.Contains(td.UserId) && !td.IsDisable));

    public Task UpdateAsync(RegisterTeams registerTeam)
    {
        _context.Set<RegisterTeams>().Update(registerTeam);
        return Task.CompletedTask;
    }

    public async Task<bool> HasOtherActiveRegistrationAsync(Guid teamId, Guid excludeRegisterTeamId)
        => await _context.Set<RegisterTeams>()
            .AnyAsync(rt => rt.TeamId == teamId
                && rt.Id != excludeRegisterTeamId
                && (rt.Status == RegisterTeamStatusEnum.Pending
                    || rt.Status == RegisterTeamStatusEnum.Approved
                    || rt.Status == RegisterTeamStatusEnum.Banned));

    public async Task<(List<RegisterTeams> Items, int TotalCount)> GetApprovedByUserIdAsync(Guid userId, string? keyword, int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Where(rt => rt.Status == RegisterTeamStatusEnum.Approved
                && rt.Team.TeamDetails.Any(td => td.UserId == userId));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rt => rt.Event.Name.ToLower().Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<RegisterTeams> Items, int TotalCount)> GetByTeamIdAsync(
        Guid teamId, RegisterTeamStatusEnum? status, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Where(rt => rt.TeamId == teamId)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(rt => rt.Status == status.Value);

        if (isDisable.HasValue)
            query = query.Where(rt => rt.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<RegisterTeams?> GetByIdWithRoundDetailsAsync(Guid id)
        => await _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Submissions)
            .FirstOrDefaultAsync(rt => rt.Id == id);

    public async Task<RegisterTeams?> GetByIdWithRoundDetailsAndScoresAsync(Guid id)
        => await _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Submissions)
                    .ThenInclude(s => s.Scores)
            .FirstOrDefaultAsync(rt => rt.Id == id);

    public async Task<(List<RegisterTeams> Items, int TotalCount)> GetApprovedByEventIdWithScoresAsync(
        Guid eventId, int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Submissions)
                    .ThenInclude(s => s.Scores)
                        .ThenInclude(sc => sc.ScoreItems)
            .Where(rt => rt.EventId == eventId
                && rt.Status == RegisterTeamStatusEnum.Approved
                && !rt.IsDisable
                && !rt.IsBanned);

        // Chỉ lấy team đã nộp bài ở ít nhất 1 round — chưa nộp ko hiển thị trong leaderboard
        query = query.Where(rt => rt.RoundDetails.Any(rd => rd.Submissions.Any()));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<int> CountByTrackIdAsync(Guid trackId)
        => await _context.Set<RegisterTeams>()
            .CountAsync(rt => rt.TrackId == trackId
                && rt.Status == RegisterTeamStatusEnum.Approved
                && !rt.IsBanned);

    public async Task<(List<RegisterTeams> Items, int TotalCount)> GetByTrackIdAsync(
        Guid trackId, string? keyword, int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Where(rt => rt.TrackId == trackId && !rt.IsDisable)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rt => rt.Team.Name.ToLower().Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<RegisterTeams> Items, int TotalCount)> GetByEventIdAndTeamIdAsync(
        Guid eventId, Guid teamId, RegisterTeamStatusEnum? status,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Where(rt => rt.EventId == eventId && rt.TeamId == teamId)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(rt => rt.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<RegisterTeams> Items, int TotalCount)> SearchAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        Guid? roundId, Guid? trackId, Guid? topicId,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Where(rt => rt.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rt => rt.Team.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(rt => rt.Status == status.Value);

        if (isBanned.HasValue)
            query = query.Where(rt => rt.IsBanned == isBanned.Value);

        if (isDisable.HasValue)
            query = query.Where(rt => rt.IsDisable == isDisable.Value);

        if (fromDate.HasValue)
            query = query.Where(rt => rt.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(rt => rt.CreatedAt <= toDate.Value);

        if (trackId.HasValue)
            query = query.Where(rt => rt.TrackId == trackId.Value);

        if (topicId.HasValue)
            query = query.Where(rt => rt.TopicId == topicId.Value);

        if (roundId.HasValue)
            query = query.Where(rt => rt.RoundDetails.Any(rd => rd.RoundId == roundId.Value && !rd.IsDisable));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<RegisterTeams> Items, int TotalCount)> SearchWithScoresAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        Guid? roundId, Guid? trackId, Guid? topicId,
        int pageIndex, int pageSize)
    {
        var query = _context.Set<RegisterTeams>()
            .Include(rt => rt.Team)
            .Include(rt => rt.Event)
            .Include(rt => rt.Track)
            .Include(rt => rt.Topic)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Round)
            .Include(rt => rt.RoundDetails)
                .ThenInclude(rd => rd.Submissions)
                    .ThenInclude(s => s.Scores)
            .Where(rt => rt.EventId == eventId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(rt => rt.Team.Name.ToLower().Contains(kw));
        }

        if (status.HasValue)
            query = query.Where(rt => rt.Status == status.Value);

        if (isBanned.HasValue)
            query = query.Where(rt => rt.IsBanned == isBanned.Value);

        if (isDisable.HasValue)
            query = query.Where(rt => rt.IsDisable == isDisable.Value);

        if (fromDate.HasValue)
            query = query.Where(rt => rt.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(rt => rt.CreatedAt <= toDate.Value);

        if (trackId.HasValue)
            query = query.Where(rt => rt.TrackId == trackId.Value);

        if (topicId.HasValue)
            query = query.Where(rt => rt.TopicId == topicId.Value);

        if (roundId.HasValue)
            query = query.Where(rt => rt.RoundDetails.Any(rd => rd.RoundId == roundId.Value && !rd.IsDisable));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(rt => rt.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
