using Hackathon.Application.Common.Helpers.Leaderboard;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Common.Models.Leaderboard;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly LeaderboardHelper _leaderboardHelper;
    private readonly IEventRepository _eventRepository;

    public Service(
        IAuthorizationService authorizationService,
        LeaderboardHelper leaderboardHelper,
        IEventRepository eventRepository)
    {
        _authorizationService = authorizationService;
        _leaderboardHelper = leaderboardHelper;
        _eventRepository = eventRepository;
    }

    public async Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var leaderBoard = await _eventRepository.GetLeaderBoardByEventIdAsync(eventId);
        if (leaderBoard == null || leaderBoard.IsDisable)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return await _leaderboardHelper.GetEventLeaderboardAsync(eventId, pageIndex, pageSize);
    }

    public async Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        return await _leaderboardHelper.GetRoundLeaderboardAsync(roundId, pageIndex, pageSize);
    }

    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        return await _leaderboardHelper.GetChapterLeaderboardAsync(year, pageIndex, pageSize);
    }
}
