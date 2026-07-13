using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Score;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Score;

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
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var score = await _scoreRepository.GetByIdAsync(scoreId);
        if (score == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var registerTeam = score.Submission?.RoundDetail?.RegisterTeam;

        return new GetScoreDetailResponse
        {
            ScoreId = score.Id,
            SubmissionId = score.SubmissionId,
            AssignTrackId = score.AssignTrackId,
            TrackTitle = score.AssignTrack?.Track?.Title,
            TrackId = registerTeam?.TrackId,
            TopicId = registerTeam?.TopicId,
            TopicTitle = registerTeam?.Topic?.Title,
            TotalScore = score.TotalScore,
            IsRetake = score.IsRetake,
            RetakeFromScoreId = score.RetakeFromScoreId,
            IsMock = score.IsMock,
            GradedBy = score.AssignTrack?.AssignEvent?.User != null
                ? new GraderInfo
                {
                    UserId = score.AssignTrack.AssignEvent.User.Id,
                    Email = score.AssignTrack.AssignEvent.User.Email,
                    FirstName = score.AssignTrack.AssignEvent.User.FirstName,
                    LastName = score.AssignTrack.AssignEvent.User.LastName
                }
                : null,
            CreatedAt = score.CreatedAt,
            UpdatedAt = score.UpdatedAt,
        };
    }

    public async Task<GetSubmissionGraderScoresResponse> GetSubmissionGraderScores(Guid submissionId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(pageIndex, pageSize);

        var (scores, totalCount) = await _scoreRepository.GetScoresBySubmissionIdAsync(submissionId, pageIndex, pageSize);

        return new GetSubmissionGraderScoresResponse
        {
            SubmissionId = submissionId,
            Scores = scores.Select(s =>
            {
                var rt = s.Submission?.RoundDetail?.RegisterTeam;
                return new ScoreDetail
                {
                    ScoreId = s.Id,
                    SubmissionId = s.SubmissionId,
                    AssignTrackId = s.AssignTrackId,
                    TrackTitle = s.AssignTrack?.Track?.Title,
                    TrackId = rt?.TrackId,
                    TopicId = rt?.TopicId,
                    TopicTitle = rt?.Topic?.Title,
                    TotalScore = s.TotalScore,
                    IsRetake = s.IsRetake,
                    RetakeFromScoreId = s.RetakeFromScoreId,
                    IsMock = s.IsMock,
                    GradedBy = s.AssignTrack?.AssignEvent?.User != null
                        ? new GraderInfo
                        {
                            UserId = s.AssignTrack.AssignEvent.User.Id,
                            Email = s.AssignTrack.AssignEvent.User.Email,
                            FirstName = s.AssignTrack.AssignEvent.User.FirstName,
                            LastName = s.AssignTrack.AssignEvent.User.LastName
                        }
                        : null,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                };
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(pageIndex, pageSize);

        var (items, totalCount) = await _scoreRepository.GetScoreItemsByScoreIdAsync(scoreId, pageIndex, pageSize);

        return new GetScoreItemsResponse
        {
            ScoreId = scoreId,
            Items = items.Select(si =>
            {
                var rt = si.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;
                return new ScoreItemDetail
                {
                    ScoreItemId = si.Id,
                    ScoreId = si.ScoreId,
                    CriteriaItemId = si.CriteriaItemId,
                    AssignTrackId = si.AssignTrackId,
                    AssignEventId = si.AssignTrack?.AssignEventId ?? Guid.Empty,
                    CriteriaName = si.CriteriaItem?.Name ?? "",
                    Score = si.Score,
                    Comment = si.Comment,
                    GradedBy = si.ScoreEntity?.AssignTrack?.AssignEvent?.User != null
                        ? new GraderInfo
                        {
                            UserId = si.ScoreEntity.AssignTrack.AssignEvent.User.Id,
                            Email = si.ScoreEntity.AssignTrack.AssignEvent.User.Email,
                            FirstName = si.ScoreEntity.AssignTrack.AssignEvent.User.FirstName,
                            LastName = si.ScoreEntity.AssignTrack.AssignEvent.User.LastName
                        }
                        : null,
                    TrackTitle = rt?.Track?.Title,
                    TrackId = rt?.TrackId,
                    TopicId = rt?.TopicId,
                    TopicTitle = rt?.Topic?.Title,
                    CreatedAt = si.CreatedAt,
                    UpdatedAt = si.UpdatedAt
                };
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var roundDetail = await _roundRepository.GetRoundDetailWithScoresAsync(roundId, registerTeamId);
        if (roundDetail == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var rt = roundDetail.RegisterTeam;
        var round = roundDetail.Round;
        var lastSubmission = SubmissionHelper.GetLastSubmission(roundDetail);

        var scopeScore = lastSubmission != null
            ? Math.Round(
                lastSubmission.Scores
                    .Where(s => s.TotalScore.HasValue)
                    .Average(s => s.TotalScore!.Value),
                2)
            : 0m;

        return new GetTeamRoundScoreResponse
        {
            RoundId = roundId,
            RegisterTeamId = registerTeamId,
            EventId = round.EventId,
            EventName = round.Event?.Name ?? "",
            TrackId = rt.TrackId,
            TrackTitle = rt.Track?.Title,
            TopicId = rt.TopicId,
            TopicTitle = rt.Topic?.Title,
            TotalScore = scopeScore,
            SubmissionId = lastSubmission?.Id,
            SubmittedAt = lastSubmission?.SubmittedAt,
            IsLastSubmission = lastSubmission != null
        };
    }

    public async Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var item = await _scoreRepository.GetScoreItemByIdAsync(scoreItemId);
        if (item == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var rt = item.ScoreEntity?.Submission?.RoundDetail?.RegisterTeam;

        return new ScoreItemDetail
        {
            ScoreItemId = item.Id,
            ScoreId = item.ScoreId,
            CriteriaItemId = item.CriteriaItemId,
            AssignTrackId = item.AssignTrackId,
            AssignEventId = item.AssignTrack?.AssignEventId ?? Guid.Empty,
            CriteriaName = item.CriteriaItem?.Name ?? "",
            Score = item.Score,
            Comment = item.Comment,
            GradedBy = item.ScoreEntity?.AssignTrack?.AssignEvent?.User != null
                ? new GraderInfo
                {
                    UserId = item.ScoreEntity.AssignTrack.AssignEvent.User.Id,
                    Email = item.ScoreEntity.AssignTrack.AssignEvent.User.Email,
                    FirstName = item.ScoreEntity.AssignTrack.AssignEvent.User.FirstName,
                    LastName = item.ScoreEntity.AssignTrack.AssignEvent.User.LastName
                }
                : null,
            TrackTitle = rt?.Track?.Title,
            TrackId = rt?.TrackId,
            TopicId = rt?.TopicId,
            TopicTitle = rt?.Topic?.Title,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}
