using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.TeamDetail;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Submission;

public class Service : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ICurrentUserService _currentUserService;

    public Service(
        ISubmissionRepository submissionRepository,
        IRegisterTeamRepository registerTeamRepository,
        ICurrentUserService currentUserService)
    {
        _submissionRepository = submissionRepository;
        _registerTeamRepository = registerTeamRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetRegisterTeamSubmissionsResponse> GetRegisterTeamSubmissions(Guid registerTeamId, Guid? roundId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null || registerTeam.IsDisable)
            throw new NotFoundException("Register Team Not Found");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            registerTeam.EventId, roundId, null, null, registerTeamId, null,
            1, int.MaxValue);

        var resultItems = items
            .Select(rd =>
            {
                var lastSubmission = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .FirstOrDefault();

                return new RegisterTeamSubmissionItem
                {
                    RegisterTeamId = registerTeam.Id,
                    TeamId = registerTeam.TeamId,
                    TeamName = registerTeam.Team?.Name ?? "",
                    EventId = registerTeam.EventId,
                    EventName = registerTeam.Event?.Name ?? "",
                    RoundId = rd.RoundId,
                    RoundName = rd.Round?.Name ?? "",
                    TrackId = registerTeam.TrackId,
                    TrackTitle = registerTeam.Track?.Title,
                    TopicId = registerTeam.TopicId,
                    TopicTitle = registerTeam.Topic?.Title,
                    SubmittedBy = GetTeamLeader(registerTeam.Team?.TeamDetails),
                    LastSubmission = lastSubmission != null
                        ? new LastSubmissionInfo
                        {
                            Id = lastSubmission.Id,
                            SubmittedAt = lastSubmission.SubmittedAt,
                            Url = lastSubmission.Url,
                            Description = lastSubmission.Description,
                            Status = lastSubmission.Status?.ToString()
                        }
                        : null,
                    ScoreId = lastSubmission?.Scores
                        .OrderByDescending(s => s.CreatedAt)
                        .FirstOrDefault()?.Id,
                    TotalScore = lastSubmission?.Scores
                        .OrderByDescending(s => s.CreatedAt)
                        .FirstOrDefault()?.TotalScore
                };
            })
            .OrderByDescending(i => i.LastSubmission?.SubmittedAt)
            .ToList();

        return new GetRegisterTeamSubmissionsResponse
        {
            Items = resultItems,
            TotalCount = resultItems.Count,
            PageIndex = 1,
            PageSize = int.MaxValue
        };
    }

    private static SubmittedByInfo? GetTeamLeader(ICollection<TeamDetails>? teamDetails)
    {
        var leader = teamDetails?.FirstOrDefault(td => td.IsLeader && !td.IsDisable && td.Status == TeamDetailStatusEnum.Active);
        if (leader?.User == null) return null;

        return new SubmittedByInfo
        {
            UserId = leader.User.Id,
            Email = leader.User.Email,
            FirstName = leader.User.FirstName,
            LastName = leader.User.LastName
        };
    }

    public async Task<SubmissionDetailResponse> GetSubmissionDetail(Guid submissionId)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var validScores = submission.Scores.Where(s => s.TotalScore.HasValue).ToList();
        var totalScore = validScores.Count > 0
            ? Math.Round(validScores.Sum(s => s.TotalScore!.Value), 2)
            : (decimal?)null;

        return new SubmissionDetailResponse
        {
            Id = submission.Id,
            RoundDetailId = submission.RoundDetailId,
            RoundId = submission.RoundDetail.RoundId,
            RoundName = submission.RoundDetail.Round.Name,
            RegisterTeamId = submission.RoundDetail.RegisterTeamId,
            TeamId = submission.RoundDetail.RegisterTeam.TeamId,
            TeamName = submission.RoundDetail.RegisterTeam.Team.Name,
            TrackId = submission.RoundDetail.RegisterTeam.TrackId,
            TrackTitle = submission.RoundDetail.RegisterTeam.Track?.Title,
            TopicId = submission.RoundDetail.RegisterTeam.TopicId,
            TopicTitle = submission.RoundDetail.RegisterTeam.Topic?.Title,
            Url = submission.Url,
            Description = submission.Description,
            Status = submission.Status?.ToString(),
            SubmittedAt = submission.SubmittedAt,
            IsRegrade = submission.IsRegrade,
            SubmittedBy = submission.RoundDetail.RegisterTeam.Team.TeamDetails
                .Where(td => td.IsLeader)
                .Select(td => new SubmittedByInfo
                {
                    UserId = td.User.Id,
                    Email = td.User.Email,
                    FirstName = td.User.FirstName,
                    LastName = td.User.LastName
                })
                .FirstOrDefault(),
            TotalScore = totalScore,
            JudgeCount = validScores.Count,
            CreatedAt = submission.CreatedAt,
            UpdatedAt = submission.UpdatedAt
        };
    }
}
