using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Leaderboard;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Common.Models.Leaderboard;
using Hackathon.Application.Exceptions;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly LeaderboardHelper _leaderboardHelper;
    private readonly IEventRepository _eventRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        LeaderboardHelper leaderboardHelper,
        IEventRepository eventRepository,
        ITeamRepository teamRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _leaderboardHelper = leaderboardHelper;
        _eventRepository = eventRepository;
        _teamRepository = teamRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    // ── API 1: Chapter leaderboard (only published) ──

    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize)
    {
        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        var publishedLeaderBoards = allLeaderBoards
            .Where(lb => lb.IsPublished && !lb.IsDisable)
            .ToList();

        return await _leaderboardHelper.GetChapterLeaderboardAsync(year, pageIndex, pageSize, publishedLeaderBoards);
    }

    // ── API 2: Event leaderboard (only IsDisable=false) ──

    public async Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize)
    {
        var leaderBoard = await _eventRepository.GetLeaderBoardByEventIdAsync(eventId);
        if (leaderBoard == null || leaderBoard.IsDisable)
            return null!;

        return await _leaderboardHelper.GetEventLeaderboardAsync(eventId, pageIndex, pageSize);
    }

    // ── API 3: My year rank ──

    public async Task<GetMyYearRankResponse> GetMyYearRank(GetMyYearRankRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var userTeamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);
        if (userTeamIds.Count == 0)
        {
            return new GetMyYearRankResponse
            {
                Year = request.Year,
                Teams = new()
            };
        }

        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(request.Year);
        var publishedLeaderBoards = allLeaderBoards
            .Where(lb => lb.IsPublished && !lb.IsDisable)
            .ToList();

        var fullResponse = await _leaderboardHelper.GetChapterLeaderboardAsync(request.Year, 1, int.MaxValue, publishedLeaderBoards);
        if (fullResponse == null)
        {
            return new GetMyYearRankResponse
            {
                Year = request.Year,
                Teams = new()
            };
        }

        var myTeams = fullResponse.Items
            .Where(i => userTeamIds.Contains(i.TeamId))
            .Select(i => new MyYearRankItem
            {
                TeamId = i.TeamId,
                TeamName = i.TeamName,
                Rank = i.Rank,
                ChapterScore = i.ChapterScore,
                EventCount = i.EventCount
            })
            .ToList();

        return new GetMyYearRankResponse
        {
            Year = request.Year,
            Teams = myTeams
        };
    }

    // ── API 4: My year detail (per-event breakdown + event rank) ──

    public async Task<GetMyYearDetailResponse> GetMyYearDetail(GetMyYearDetailRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var userTeamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);
        if (userTeamIds.Count == 0)
        {
            return new GetMyYearDetailResponse
            {
                Year = request.Year,
                Teams = new()
            };
        }

        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(request.Year);
        var publishedLeaderBoards = allLeaderBoards
            .Where(lb => lb.IsPublished && !lb.IsDisable)
            .ToList();

        var fullResponse = await _leaderboardHelper.GetChapterLeaderboardAsync(request.Year, 1, int.MaxValue, publishedLeaderBoards);
        if (fullResponse == null)
        {
            return new GetMyYearDetailResponse
            {
                Year = request.Year,
                Teams = new()
            };
        }

        // Build per-event rank map: eventId -> { teamId -> rank }
        var eventRankMap = new Dictionary<Guid, Dictionary<Guid, int>>();
        foreach (var lb in publishedLeaderBoards)
        {
            if (lb.Event == null) continue;

            var eventLb = await _leaderboardHelper.GetEventLeaderboardAsync(lb.EventId, 1, int.MaxValue);
            var rankMap = new Dictionary<Guid, int>();
            foreach (var item in eventLb.Items)
            {
                if (!rankMap.ContainsKey(item.TeamId))
                    rankMap[item.TeamId] = item.Rank;
            }
            eventRankMap[lb.EventId] = rankMap;
        }

        var myTeams = fullResponse.Items
            .Where(i => userTeamIds.Contains(i.TeamId))
            .Select(i =>
            {
                var eventDetails = i.EventScores.Select(e => new EventScoreRankDetail
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    RegisterTeamId = e.RegisterTeamId,
                    EventScore = e.EventScore,
                    EventRank = eventRankMap.TryGetValue(e.EventId, out var rankMap)
                        && rankMap.TryGetValue(i.TeamId, out var rank)
                            ? rank
                            : 0
                }).ToList();

                return new MyYearDetailTeamItem
                {
                    TeamId = i.TeamId,
                    TeamName = i.TeamName,
                    Rank = i.Rank,
                    ChapterScore = i.ChapterScore,
                    EventScores = eventDetails
                };
            })
            .ToList();

        return new GetMyYearDetailResponse
        {
            Year = request.Year,
            Teams = myTeams
        };
    }

    // ── API 5: My event rank ──

    public async Task<GetMyEventRankResponse> GetMyEventRank(GetMyEventRankRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var leaderBoard = await _eventRepository.GetLeaderBoardByEventIdAsync(request.EventId);
        if (leaderBoard == null || leaderBoard.IsDisable)
            return null!;

        var userTeamIds = await _teamRepository.GetUserActiveTeamIdsAsync(userId);

        var fullResponse = await _leaderboardHelper.GetEventLeaderboardAsync(request.EventId, 1, int.MaxValue);
        if (fullResponse == null)
        {
            return new GetMyEventRankResponse
            {
                EventId = request.EventId,
                EventName = "",
                Teams = new()
            };
        }

        var myTeams = fullResponse.Items
            .Where(i => userTeamIds.Contains(i.TeamId))
            .Select(i => new MyEventRankItem
            {
                Rank = i.Rank,
                RegisterTeamId = i.RegisterTeamId,
                TeamId = i.TeamId,
                TeamName = i.TeamName,
                EventScore = i.EventScore,
                RoundScores = i.RoundScores
            })
            .ToList();

        return new GetMyEventRankResponse
        {
            EventId = request.EventId,
            EventName = fullResponse.EventName,
            Teams = myTeams
        };
    }
}
