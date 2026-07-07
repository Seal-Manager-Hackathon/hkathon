using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

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

    public async Task<GetScoreDetailResponse> GetScoreDetail(Guid scoreId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetScoreDetailResponse
        {
            ScoreId = score.Id,
            SubmissionId = score.SubmissionId,
            AssignTrackId = score.AssignTrackId,
            TrackTitle = score.AssignTrack?.Track?.Title,
            TotalScore = score.TotalScore,
            IsRetake = score.IsRetake,
            RetakeFromScoreId = score.RetakeFromScoreId,
            IsMock = score.IsMock,
            CreatedAt = score.CreatedAt,
            UpdatedAt = score.UpdatedAt,
            Items = score.ScoreItems.Select(si => new ScoreItemDetail
            {
                ScoreItemId = si.Id,
                ScoreId = si.ScoreId,
                CriteriaItemId = si.CriteriaItemId,
                AssignTrackId = si.AssignTrackId,
                AssignEventId = si.AssignTrack?.AssignEventId ?? Guid.Empty,
                CriteriaName = si.CriteriaItem?.Name ?? "",
                Score = si.Score,
                Comment = si.Comment,
                GradedBy = si.AssignTrack?.AssignEvent?.User != null
                    ? new GraderInfo
                    {
                        UserId = si.AssignTrack.AssignEvent.User.Id,
                        Email = si.AssignTrack.AssignEvent.User.Email,
                        FirstName = si.AssignTrack.AssignEvent.User.FirstName,
                        LastName = si.AssignTrack.AssignEvent.User.LastName
                    }
                    : null,
                CreatedAt = si.CreatedAt,
                UpdatedAt = si.UpdatedAt
            }).ToList()
        };
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
                SubmissionId = s.SubmissionId,
                AssignTrackId = s.AssignTrackId,
                TrackTitle = s.AssignTrack?.Track?.Title,
                TotalScore = s.TotalScore,
                IsRetake = s.IsRetake,
                RetakeFromScoreId = s.RetakeFromScoreId,
                IsMock = s.IsMock,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
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
                ScoreId = si.ScoreId,
                CriteriaItemId = si.CriteriaItemId,
                AssignTrackId = si.AssignTrackId,
                AssignEventId = si.AssignTrack?.AssignEventId ?? Guid.Empty,
                CriteriaName = si.CriteriaItem?.Name ?? "",
                Score = si.Score,
                Comment = si.Comment,
                GradedBy = si.AssignTrack?.AssignEvent?.User != null
                    ? new GraderInfo
                    {
                        UserId = si.AssignTrack.AssignEvent.User.Id,
                        Email = si.AssignTrack.AssignEvent.User.Email,
                        FirstName = si.AssignTrack.AssignEvent.User.FirstName,
                        LastName = si.AssignTrack.AssignEvent.User.LastName
                    }
                    : null,
                CreatedAt = si.CreatedAt,
                UpdatedAt = si.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
