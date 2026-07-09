using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ISubmissionRepository submissionRepository,
        IRoundRepository roundRepository,
        IRegisterTeamRepository registerTeamRepository,
        IEventRepository eventRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _submissionRepository = submissionRepository;
        _roundRepository = roundRepository;
        _registerTeamRepository = registerTeamRepository;
        _eventRepository = eventRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    private async Task EnsureStaffAssignedToEvent(Guid eventId)
    {
        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);
    }

    public async Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        // Staff phải được assign vào event của round này
        await EnsureStaffAssignedToEvent(round.EventId);

        var (items, totalCount) = await _submissionRepository.GetRoundSummaryAsync(roundId, pageIndex, pageSize);

        var rankedItems = items.Select((item, index) => new RoundLeaderboardItem
        {
            Rank = ((pageIndex - 1) * pageSize) + index + 1,
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

    public async Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var eventEntity = await _eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Staff phải được assign vào event này
        await EnsureStaffAssignedToEvent(eventId);

        var (teams, totalCount) = await _registerTeamRepository.GetApprovedByEventIdWithScoresAsync(eventId, pageIndex, pageSize);

        // Đếm tổng số round của event
        var totalRounds = teams.FirstOrDefault()?.RoundDetails
            .Select(rd => rd.Round)
            .Where(r => !r.IsDisable)
            .Distinct()
            .Count() ?? 0;

        var items = teams.Select((rt, index) =>
        {
            // Tính scopeScore cho từng round
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

            // eventScore = weighted avg (weight_i=1) — denominator = totalRounds
            var eventScore = EventScoreHelper.Calculate(
                roundScores.Select(r => r.ScopeScore).ToList(),
                totalRounds > 0 ? totalRounds : roundScores.Count);

            return new EventLeaderboardItem
            {
                Rank = ((pageIndex - 1) * pageSize) + index + 1,
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

    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Chỉ lấy event mà staff được assign trong năm
        var allEvents = await _eventRepository.GetPublishedByYearAsync(year);
        var assignedEventIds = new List<Guid>();

        foreach (var ev in allEvents)
        {
            var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(ev.Id, currentUserId);
            if (assignEvent != null)
                assignedEventIds.Add(ev.Id);
        }

        var events = allEvents.Where(e => assignedEventIds.Contains(e.Id)).ToList();

        // Gom nhóm theo TeamId
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

        var allTeams = teamDict.Select((kv, index) =>
        {
            var eventScores = kv.Value.EventScores;
            var chapterScore = ChapterScoreHelper.Calculate(
                eventScores.Select(e => e.EventScore).ToList());

            return new ChapterLeaderboardItem
            {
                Rank = index + 1,
                TeamId = kv.Key,
                TeamName = kv.Value.Name,
                ChapterScore = chapterScore,
                EventCount = eventScores.Count,
                EventScores = eventScores.OrderBy(e => e.EventName).ToList()
            };
        })
        .OrderByDescending(x => x.ChapterScore)
        .ToList();

        var totalCount = allTeams.Count;
        var pagedItems = allTeams
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .Select((item, idx) =>
            {
                item.Rank = ((pageIndex - 1) * pageSize) + idx + 1;
                return item;
            })
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
    /// Tính scopeScore = SUM(Scores.TotalScore) của submission cuối cùng trong round.
    /// </summary>
    private static decimal CalculateScopeScore(RoundDetails rd)
    {
        var lastSubmission = SubmissionHelper.GetLastSubmission(rd);

        if (lastSubmission == null) return 0;

        return RoundScoreHelper.Calculate(lastSubmission).Total;
    }
}