using Hackathon.Application.Common.Helpers.Leaderboard;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.Models.Leaderboard;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Admin.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly LeaderboardHelper _leaderboardHelper;

    public Service(
        IAuthorizationService authorizationService,
        LeaderboardHelper leaderboardHelper)
    {
        _authorizationService = authorizationService;
        _leaderboardHelper = leaderboardHelper;
    }

    public async Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        return await _leaderboardHelper.GetRoundLeaderboardAsync(roundId, pageIndex, pageSize);
    }

    public async Task<GetEventLeaderboardResponse> GetEventLeaderboard(Guid eventId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        return await _leaderboardHelper.GetEventLeaderboardAsync(eventId, pageIndex, pageSize);
    }

    public async Task<GetChapterLeaderboardResponse> GetChapterLeaderboard(int year, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        return await _leaderboardHelper.GetChapterLeaderboardAsync(year, pageIndex, pageSize);
    }

    public async Task PublishChapter(int year)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        await _leaderboardHelper.PublishChapterAsync(year);
    }

    public async Task HideChapter(int year)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        await _leaderboardHelper.HideChapterAsync(year);
    }

    public async Task PublishEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        await _leaderboardHelper.PublishEventAsync(eventId);
    }

    public async Task HideEvent(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        await _leaderboardHelper.HideEventAsync(eventId);
    }
}
