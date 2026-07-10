using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Common.Models.Leaderboard;
using Hackathon.Domain.Enums.RegisterTeam;

namespace Hackathon.Application.Common.Helpers.Leaderboard;

/// <summary>
/// Helper tính leaderboard cho round, event và chapter.
/// Dùng chung cho cả Admin và Staff — chỉ khác authorization layer bên ngoài.
/// Tất cả rank đều tính theo DENSE_RANK: cùng điểm → cùng rank, rank tiếp = rank trước + 1.
/// </summary>
public class LeaderboardHelper
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LeaderboardHelper(
        ISubmissionRepository submissionRepository,
        IRoundRepository roundRepository,
        IRegisterTeamRepository registerTeamRepository,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork)
    {
        _submissionRepository = submissionRepository;
        _roundRepository = roundRepository;
        _registerTeamRepository = registerTeamRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Tính round leaderboard — xếp hạng theo TotalScore DESC (DENSE_RANK).
    /// </summary>
    public async Task<GetRoundLeaderboardResponse> GetRoundLeaderboardAsync(Guid roundId, int pageIndex, int pageSize)
    {
        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            return null!;

        var (items, totalCount) = await _submissionRepository.GetRoundSummaryAsync(roundId, pageIndex, pageSize);

        var rankedItems = items.Select(item => new RoundLeaderboardItem
        {
            RegisterTeamId = item.RegisterTeamId,
            TeamId = item.TeamId,
            TeamName = item.TeamName,
            TrackId = item.TrackId,
            TrackTitle = item.TrackTitle,
            TopicId = item.TopicId,
            TopicTitle = item.TopicTitle,
            LastSubmissionId = item.LastSubmissionId,
            TotalScore = item.TotalScore
        }).ToList();

        AssignDenseRank(rankedItems, i => i.TotalScore);

        return new GetRoundLeaderboardResponse
        {
            RoundId = roundId,
            RoundName = round.Name,
            EventId = round.EventId,
            EventName = round.Event?.Name ?? "",
            Items = rankedItems,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Tính event leaderboard — tính scopeScore từng round → eventScore → xếp hạng (DENSE_RANK).
    /// </summary>
    public async Task<GetEventLeaderboardResponse> GetEventLeaderboardAsync(Guid eventId, int pageIndex, int pageSize)
    {
        var (teams, totalCount) = await _registerTeamRepository.GetApprovedByEventIdWithScoresAsync(eventId, pageIndex, pageSize);

        var totalRounds = teams.FirstOrDefault()?.RoundDetails
            .Select(rd => rd.Round)
            .Where(r => !r.IsDisable)
            .Distinct()
            .Count() ?? 0;

        var items = teams
            .Select(rt =>
            {
                var roundScores = rt.RoundDetails
                    .Where(rd => !rd.IsDisable && !rd.Round.IsDisable)
                    .Select(rd =>
                    {
                        var scopeScore = CalculateScopeScore(rd);
                        return new RoundScoreDetail
                        {
                            RoundNo = rd.Round.RoundNo ?? 0,
                            RoundName = rd.Round.Name,
                            ScopeScore = scopeScore
                        };
                    })
                    .OrderBy(r => r.RoundNo)
                    .ToList();

                var eventScore = EventScoreHelper.Calculate(
                    roundScores.Select(r => r.ScopeScore).ToList(),
                    totalRounds > 0 ? totalRounds : roundScores.Count);

                return new EventLeaderboardItem
                {
                    RegisterTeamId = rt.Id,
                    TeamId = rt.TeamId,
                    TeamName = rt.Team?.Name ?? "",
                    TrackId = rt.TrackId,
                    TrackTitle = rt.Track?.Title,
                    TopicId = rt.TopicId,
                    TopicTitle = rt.Topic?.Title,
                    EventScore = eventScore,
                    RoundScores = roundScores
                };
            })
            .OrderByDescending(x => x.EventScore)
            .ToList();

        AssignDenseRank(items, i => i.EventScore);

        return new GetEventLeaderboardResponse
        {
            EventId = eventId,
            EventName = teams.FirstOrDefault()?.Event?.Name ?? "",
            TotalRounds = totalRounds,
            Items = items,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Tính chapter leaderboard — gom team qua các event → chapterScore → xếp hạng (DENSE_RANK).
    /// Chỉ tính event có LeaderBoards với year trùng, status Published/Closed, IsDisable=false.
    /// filteredLeaderBoards: nếu truyền vào thì chỉ tính trên các leader board đó (dùng cho Staff bị giới hạn event).
    /// </summary>
    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboardAsync(int year, int pageIndex, int pageSize, List<Domain.Entities.LeaderBoards>? filteredLeaderBoards = null)
    {
        var leaderBoards = filteredLeaderBoards ?? await _eventRepository.GetLeaderBoardByYearAsync(year);
        var events = leaderBoards.Where(lb => lb.Event != null).Select(lb => lb.Event).ToList();

        var teamDict = new Dictionary<Guid, (string Name, List<EventScoreDetail> EventScores)>();
        foreach (var ev in events)
        {
            var approvedTeams = ev.RegisterTeams
                .Where(rt => rt.Status == RegisterTeamStatusEnum.Approved && !rt.IsDisable);

            foreach (var rt in approvedTeams)
            {
                var totalRounds = ev.Rounds.Count(r => !r.IsDisable);
                var roundScores = rt.RoundDetails
                    .Where(rd => !rd.IsDisable && !rd.Round.IsDisable)
                    .Select(rd => CalculateScopeScore(rd))
                    .ToList();

                var eventScore = EventScoreHelper.Calculate(roundScores, totalRounds);

                if (!teamDict.ContainsKey(rt.TeamId))
                    teamDict[rt.TeamId] = (rt.Team?.Name ?? "", new());

                teamDict[rt.TeamId].EventScores.Add(new EventScoreDetail
                {
                    EventId = ev.Id,
                    EventName = ev.Name,
                    RegisterTeamId = rt.Id,
                    EventScore = eventScore
                });
            }
        }

        var allTeams = teamDict.Select(kv =>
        {
            var eventScores = kv.Value.EventScores;
            var chapterScore = ChapterScoreHelper.Calculate(
                eventScores.Select(e => e.EventScore).ToList());

            return new ChapterLeaderboardItem
            {
                TeamId = kv.Key,
                TeamName = kv.Value.Name,
                ChapterScore = chapterScore,
                EventCount = eventScores.Count,
                EventScores = eventScores.OrderBy(e => e.EventName).ToList()
            };
        })
        .OrderByDescending(x => x.ChapterScore)
        .ToList();

        AssignDenseRank(allTeams, i => i.ChapterScore);

        var totalCount = allTeams.Count;
        var pagedItems = allTeams
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetChapterLeaderboardResponse
        {
            Year = year,
            EventCount = events.Count,
            Items = pagedItems,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Publish chapter leaderboard — set IsDisable=false, IsPublished=true cho tất cả leader board trong năm.
    /// </summary>
    public async Task<int> PublishChapterAsync(int year, List<Guid>? allowedEventIds = null)
    {
        var leaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        if (allowedEventIds != null)
        {
            leaderBoards = leaderBoards.Where(lb => allowedEventIds.Contains(lb.EventId)).ToList();
        }

        foreach (var lb in leaderBoards)
        {
            lb.IsDisable = false;
            lb.IsPublished = true;
            lb.UpdatedAt = DateTimeOffset.UtcNow;
        }

        return await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Publish event leaderboard — set IsDisable=false, IsPublished=true cho leader board của event.
    /// </summary>
    public async Task<int> PublishEventAsync(Guid eventId)
    {
        var lb = await _eventRepository.GetLeaderBoardByEventIdAsync(eventId);
        if (lb == null)
            return 0;

        lb.IsDisable = false;
        lb.IsPublished = true;
        lb.UpdatedAt = DateTimeOffset.UtcNow;

        await _eventRepository.UpdateLeaderBoardAsync(lb);
        return await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Hide (soft-delete) event leaderboard — set IsDisable=true cho leader board của event.
    /// </summary>
    public async Task<int> HideEventAsync(Guid eventId)
    {
        var lb = await _eventRepository.GetLeaderBoardByEventIdAsync(eventId);
        if (lb == null)
            return 0;

        lb.IsDisable = true;
        lb.UpdatedAt = DateTimeOffset.UtcNow;

        await _eventRepository.UpdateLeaderBoardAsync(lb);
        return await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// Hide (soft-delete) chapter leaderboard — set IsDisable=true cho tất cả leader board trong năm.
    /// </summary>
    public async Task<int> HideChapterAsync(int year, List<Guid>? allowedEventIds = null)
    {
        var leaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        if (allowedEventIds != null)
        {
            leaderBoards = leaderBoards.Where(lb => allowedEventIds.Contains(lb.EventId)).ToList();
        }

        foreach (var lb in leaderBoards)
        {
            lb.IsDisable = true;
            lb.UpdatedAt = DateTimeOffset.UtcNow;
        }

        return await _unitOfWork.SaveChangesAsync();
    }

    /// <summary>
    /// DENSE_RANK: items phải được sort DESC theo score trước khi gọi.
    /// Cùng score → cùng rank, rank tiếp theo = rank trước + 1 (không nhảy).
    /// </summary>
    private static void AssignDenseRank<T>(List<T> items, Func<T, decimal?> scoreSelector) where T : class
    {
        if (items.Count == 0) return;

        var rankProp = typeof(T).GetProperty("Rank");
        if (rankProp == null) return;

        var rank = 1;
        rankProp.SetValue(items[0], rank);
        var prevScore = scoreSelector(items[0]);

        for (int i = 1; i < items.Count; i++)
        {
            var currentScore = scoreSelector(items[i]);

            if (currentScore != prevScore)
                rank++;

            rankProp.SetValue(items[i], rank);
            prevScore = currentScore;
        }
    }

    /// <summary>
    /// DENSE_RANK overload cho score non-nullable.
    /// </summary>
    private static void AssignDenseRank<T>(List<T> items, Func<T, decimal> scoreSelector) where T : class
    {
        if (items.Count == 0) return;

        var rankProp = typeof(T).GetProperty("Rank");
        if (rankProp == null) return;

        var rank = 1;
        rankProp.SetValue(items[0], rank);
        var prevScore = scoreSelector(items[0]);

        for (int i = 1; i < items.Count; i++)
        {
            var currentScore = scoreSelector(items[i]);

            if (currentScore != prevScore)
                rank++;

            rankProp.SetValue(items[i], rank);
            prevScore = currentScore;
        }
    }

    private static decimal CalculateScopeScore(Domain.Entities.RoundDetails rd)
    {
        var lastSubmission = SubmissionHelper.GetLastSubmission(rd);
        if (lastSubmission == null) return 0;
        return RoundScoreHelper.Calculate(lastSubmission).Total;
    }
}
