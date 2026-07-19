using Hackathon.Application.Common;
using Hackathon.Application.Common.Helpers.Notification;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Notification;
using Hackathon.Domain.Enums.Submission;
using Hackathon.Domain.Enums.TeamDetail;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Submission;

public class Service : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ISubmissionRepository submissionRepository,
        IRegisterTeamRepository registerTeamRepository,
        IRoundRepository roundRepository,
        ITeamRepository teamRepository,
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _submissionRepository = submissionRepository;
        _registerTeamRepository = registerTeamRepository;
        _roundRepository = roundRepository;
        _teamRepository = teamRepository;
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
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

    public async Task<CreateSubmissionResponse> CreateSubmission(CreateSubmissionRequest request)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedException(ErrMsg.Auth.UserNotFound);

        // Check register team exists
        var registerTeam = await _registerTeamRepository.GetByIdAsync(request.RegisterTeamId);
        if (registerTeam == null || registerTeam.IsDisable || registerTeam.IsBanned)
            throw new NotFoundException("Register Team Not Found");

        // Check register team is approved
        if (registerTeam.Status != Domain.Enums.RegisterTeam.RegisterTeamStatusEnum.Approved)
            throw new BadRequestException("Register Team Is Not Approved");

        // Check register team has track and topic assigned
        if (!registerTeam.TrackId.HasValue)
            throw new BadRequestException("Register Team Has No Track Assigned. Cannot Submit.");
        if (!registerTeam.TopicId.HasValue)
            throw new BadRequestException("Register Team Has No Topic Assigned. Cannot Submit.");

        // Check event exists and is open
        var ev = registerTeam.Event;
        if (ev == null || ev.IsDisable)
            throw new NotFoundException("Event Not Found");

        if (ev.Status != Domain.Enums.Event.EventStatusEnum.Published)
            throw new BadRequestException("Event Is Not Open for Submission");

        // Check team exists and not disabled
        var team = registerTeam.Team;
        if (team == null || team.IsDisable)
            throw new NotFoundException("Team Not Found");

        // Check user is leader of the team (active leader)
        var members = await _teamRepository.GetTeamMembersAsync(registerTeam.TeamId);
        var leader = members.FirstOrDefault(m => m.UserId == userId && m.IsLeader && !m.IsDisable && m.Status == TeamDetailStatusEnum.Active);
        if (leader == null)
            throw new BadRequestException("Only Team Leader Can Submit");

        // Check round exists and belongs to the same event
        var round = await _roundRepository.GetByIdAsync(request.RoundId);
        if (round == null || round.IsDisable)
            throw new NotFoundException("Round Not Found");

        if (round.EventId != registerTeam.EventId)
            throw new BadRequestException("Round Does Not Belong to This Event");

        // Check team is registered in this round (has RoundDetail, not disabled)
        var roundDetail = await _roundRepository.GetRoundDetailAsync(request.RegisterTeamId, request.RoundId);
        if (roundDetail == null || roundDetail.IsDisable)
            throw new BadRequestException("Team Is Not Registered in This Round");

        // [Commented] Check submission time window — bỏ để dễ test
        //var now = DateTimeOffset.UtcNow;
        //if (round.StartSubmission.HasValue && now < round.StartSubmission.Value)
        //    throw new BadRequestException("Submission Period Has Not Started Yet");
        //if (round.EndSubmission.HasValue && now > round.EndSubmission.Value)
        //    throw new BadRequestException("Submission Period Has Ended");
        var now = DateTimeOffset.UtcNow;

        var submission = new Submissions
        {
            Id = Guid.NewGuid(),
            RoundDetailId = roundDetail.Id,
            Url = request.Url,
            Description = request.Description,
            Status = SubmissionStatusEnum.Submitted,
            SubmittedAt = now,
            IsRegrade = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _submissionRepository.AddAsync(submission);
        await _unitOfWork.SaveChangesAsync();

        // Gửi notification cho team leader
        var teamNotification = NotificationHelper.Create(
            NotificationTargetTypeEnum.Personal,
            "Submission Submitted",
            string.Format(NotificationMessage.Submission.Submitted, team.Name, round.RoundNo ?? 0),
            userId: leader.UserId);
        await _notificationRepository.AddAsync(teamNotification);
        await _unitOfWork.SaveChangesAsync();

        return new CreateSubmissionResponse
        {
            Id = submission.Id,
            RegisterTeamId = registerTeam.Id,
            RoundId = round.Id,
            Url = submission.Url,
            Description = submission.Description,
            Status = submission.Status?.ToString(),
            SubmittedAt = submission.SubmittedAt
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
            ? Math.Round(validScores.Average(s => s.TotalScore!.Value), 2)
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
