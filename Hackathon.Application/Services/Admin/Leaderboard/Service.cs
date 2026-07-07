using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Leaderboard;

public class Service : ILeaderboardService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ISubmissionRepository submissionRepository,
        IRoundRepository roundRepository,
        IAuthorizationService authorizationService)
    {
        _submissionRepository = submissionRepository;
        _roundRepository = roundRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRoundLeaderboardResponse> GetRoundLeaderboard(Guid roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

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
}