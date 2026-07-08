using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Admin.Score;

public class Service : IScoreService
{
    private readonly IScoreRepository _scoreRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(IScoreRepository scoreRepository, IRoundRepository roundRepository, IAuthorizationService authorizationService)
    {
        _scoreRepository = scoreRepository;
        _roundRepository = roundRepository;
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
        };
    }

    public async Task<GetSubmissionScoresResponse> GetSubmissionScores(Guid submissionId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var scores = await _scoreRepository.GetBySubmissionIdAsync(submissionId);
        var validScores = scores.Where(s => s.TotalScore.HasValue).ToList();

        return new GetSubmissionScoresResponse
        {
            SubmissionId = submissionId,
            TotalScore = Math.Round(validScores.Sum(s => s.TotalScore!.Value), 2),
            JudgeCount = validScores.Count
        };
    }

    public async Task<GetSubmissionGraderScoresResponse> GetSubmissionGraderScores(Guid submissionId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var (scores, totalCount) = await _scoreRepository.GetScoresBySubmissionIdAsync(submissionId, pageIndex, pageSize);

        return new GetSubmissionGraderScoresResponse
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
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
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

    public async Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var roundDetail = await _roundRepository.GetRoundDetailWithScoresAsync(roundId, registerTeamId);
        if (roundDetail == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var registerTeam = roundDetail.RegisterTeam;
        var round = roundDetail.Round;
        // scopeScore = SUM(Scores.TotalScore) của submission cuối cùng
        var lastSubmission = SubmissionHelper.GetLastSubmission(roundDetail);

        var scopeScore = lastSubmission != null
            ? Math.Round(
                lastSubmission.Scores
                    .Where(s => s.TotalScore.HasValue)
                    .Sum(s => s.TotalScore!.Value),
                2)
            : 0m;

        return new GetTeamRoundScoreResponse
        {
            RoundId = roundId,
            RegisterTeamId = registerTeamId,
            EventId = round.EventId,
            EventName = round.Event?.Name ?? "",
            TrackId = registerTeam.TrackId,
            TrackTitle = registerTeam.Track?.Title,
            TopicId = registerTeam.TopicId,
            TopicTitle = registerTeam.Topic?.Title,
            TotalScore = scopeScore,
            SubmissionId = lastSubmission?.Id,
            SubmittedAt = lastSubmission?.SubmittedAt,
            IsLastSubmission = lastSubmission != null
        };
    }

    public async Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var scoreItem = await _scoreRepository.GetScoreItemByIdAsync(scoreItemId);
        if (scoreItem == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new ScoreItemDetail
        {
            ScoreItemId = scoreItem.Id,
            ScoreId = scoreItem.ScoreId,
            CriteriaItemId = scoreItem.CriteriaItemId,
            AssignTrackId = scoreItem.AssignTrackId,
            AssignEventId = scoreItem.AssignTrack?.AssignEventId ?? Guid.Empty,
            CriteriaName = scoreItem.CriteriaItem?.Name ?? "",
            Score = scoreItem.Score,
            Comment = scoreItem.Comment,
            GradedBy = scoreItem.AssignTrack?.AssignEvent?.User != null
                ? new GraderInfo
                {
                    UserId = scoreItem.AssignTrack.AssignEvent.User.Id,
                    Email = scoreItem.AssignTrack.AssignEvent.User.Email,
                    FirstName = scoreItem.AssignTrack.AssignEvent.User.FirstName,
                    LastName = scoreItem.AssignTrack.AssignEvent.User.LastName
                }
                : null,
            CreatedAt = scoreItem.CreatedAt,
            UpdatedAt = scoreItem.UpdatedAt
        };
    }
}
