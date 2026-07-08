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

    public async Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var roundDetail = await _roundRepository.GetRoundDetailWithScoresAsync(roundId, registerTeamId);
        if (roundDetail == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var registerTeam = roundDetail.RegisterTeam;
        var round = roundDetail.Round;
        var lastSubmission = roundDetail.Submissions
            .OrderByDescending(s => s.SubmittedAt)
            .FirstOrDefault();

        // Tính criteriaAverages từ ScoreItems (giống RoundScoreHelper)
        var allScoreItems = lastSubmission?.Scores
            .SelectMany(s => s.ScoreItems)
            .Where(si => si.Score.HasValue)
            .ToList() ?? new();

        var criteriaAverages = allScoreItems
            .GroupBy(si => new { si.CriteriaItemId, Name = si.CriteriaItem?.Name ?? "" })
            .Select(g => new TeamRoundCriteriaScore
            {
                CriteriaItemId = g.Key.CriteriaItemId,
                CriteriaName = g.Key.Name,
                AverageScore = Math.Round(g.Average(si => si.Score!.Value), 2),
                JudgeCount = g.Select(si => si.AssignTrack?.AssignEvent?.UserId).Distinct().Count()
            })
            .ToList();

        // Tính tổng criteriaAvg (scopeScore) — ko lưu, chỉ tính on-the-fly
        var scopeScore = lastSubmission != null
            ? Math.Round(criteriaAverages.Sum(c => c.AverageScore), 2)
            : 0m;

        // Thông tin từng judge (grader) score
        var graderScores = lastSubmission?.Scores
            .Select(s => new TeamRoundGraderScore
            {
                ScoreId = s.Id,
                AssignTrackId = s.AssignTrackId,
                TrackTitle = s.AssignTrack?.Track?.Title,
                TotalScore = s.TotalScore,
                IsRetake = s.IsRetake,
                IsMock = s.IsMock,
                GraderName = s.AssignTrack?.AssignEvent?.User != null
                    ? $"{s.AssignTrack.AssignEvent.User.FirstName} {s.AssignTrack.AssignEvent.User.LastName}".Trim()
                    : null,
                GraderEmail = s.AssignTrack?.AssignEvent?.User?.Email,
                CreatedAt = s.CreatedAt
            })
            .ToList() ?? new();

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
            GraderScores = graderScores,
            CriteriaAverages = criteriaAverages
        };
    }
}
