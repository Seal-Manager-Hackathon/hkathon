using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Admin.Score;

public class Service : IScoreService
{
    private readonly IScoreRepository _scoreRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(IScoreRepository scoreRepository, IAuthorizationService authorizationService)
    {
        _scoreRepository = scoreRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetSubmissionScoresResponse> GetSubmissionScores(Guid submissionId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var scores = await _scoreRepository.GetBySubmissionIdAsync(submissionId);

        return new GetSubmissionScoresResponse
        {
            SubmissionId = submissionId,
            Scores = scores.Select(s => new ScoreDetail
            {
                ScoreId = s.Id,
                AssignTrackId = s.AssignTrackId,
                TrackTitle = s.AssignTrack?.Track?.Title,
                TotalScore = s.TotalScore,
                IsRetake = s.IsRetake,
                IsMock = s.IsMock,
                Items = s.ScoreItems.Select(si => new ScoreItemDetail
                {
                    ScoreItemId = si.Id,
                    CriteriaItemId = si.CriteriaItemId,
                    CriteriaName = si.CriteriaItem?.Name ?? "",
                    Score = si.Score,
                    Comment = si.Comment
                }).ToList()
            }).ToList()
        };
    }

    public async Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var (items, totalCount) = await _scoreRepository.GetScoreItemsByScoreIdAsync(scoreId, pageIndex, pageSize);

        return new GetScoreItemsResponse
        {
            ScoreId = scoreId,
            Items = items.Select(si => new ScoreItemDetail
            {
                ScoreItemId = si.Id,
                CriteriaItemId = si.CriteriaItemId,
                CriteriaName = si.CriteriaItem?.Name ?? "",
                Score = si.Score,
                Comment = si.Comment
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
