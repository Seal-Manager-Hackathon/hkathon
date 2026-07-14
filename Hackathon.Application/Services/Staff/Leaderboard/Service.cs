using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Helpers.Leaderboard;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Common.Models.Leaderboard;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IRoundRepository _roundRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEventRepository _eventRepository;
    private readonly LeaderboardHelper _leaderboardHelper;

    public Service(
        IAuthorizationService authorizationService,
        IRoundRepository roundRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IEventRepository eventRepository,
        LeaderboardHelper leaderboardHelper)
    {
        _authorizationService = authorizationService;
        _roundRepository = roundRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _eventRepository = eventRepository;
        _leaderboardHelper = leaderboardHelper;
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

        await EnsureStaffAssignedToEvent(round.EventId);

        return await _leaderboardHelper.GetRoundLeaderboardAsync(roundId, pageIndex, pageSize);
    }

    public async Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var eventEntity = await _eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        await EnsureStaffAssignedToEvent(eventId);

        var leaderBoard = await _eventRepository.GetLeaderBoardByEventIdAsync(eventId);
        if (leaderBoard == null || leaderBoard.IsDisable || !leaderBoard.IsPublished)
            return null!;

        return await _leaderboardHelper.GetEventLeaderboardAsync(eventId, pageIndex, pageSize);
    }

    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        var publishedLeaderBoards = allLeaderBoards
            .Where(lb => lb.IsPublished && !lb.IsDisable)
            .ToList();

        return await _leaderboardHelper.GetChapterLeaderboardAsync(year, pageIndex, pageSize, publishedLeaderBoards);
    }

    public async Task PublishChapter(int year)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        var assignedEventIds = new List<Guid>();

        foreach (var lb in allLeaderBoards)
        {
            var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(lb.EventId, currentUserId);
            if (assignEvent != null)
                assignedEventIds.Add(lb.EventId);
        }

        if (assignedEventIds.Count == 0)
            throw new ForbiddenException("You Are Not Assigned to Any Event In This Year");

        await _leaderboardHelper.PublishChapterAsync(year, assignedEventIds);
    }

    public async Task HideChapter(int year)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId
            ?? throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var allLeaderBoards = await _eventRepository.GetLeaderBoardByYearAsync(year);
        var assignedEventIds = new List<Guid>();

        foreach (var lb in allLeaderBoards)
        {
            var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(lb.EventId, currentUserId);
            if (assignEvent != null)
                assignedEventIds.Add(lb.EventId);
        }

        if (assignedEventIds.Count == 0)
            throw new ForbiddenException("You Are Not Assigned to Any Event In This Year");

        await _leaderboardHelper.HideChapterAsync(year, assignedEventIds);
    }

    public async Task PublishEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await EnsureStaffAssignedToEvent(eventId);

        await _leaderboardHelper.PublishEventAsync(eventId);
    }

    public async Task HideEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        await EnsureStaffAssignedToEvent(eventId);

        await _leaderboardHelper.HideEventAsync(eventId);
    }
}
