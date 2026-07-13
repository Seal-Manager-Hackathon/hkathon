using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using Hackathon.Domain.Entities;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Submission;

public class Service : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IRoundRepository _roundRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ISubmissionRepository submissionRepository,
        IEventRepository eventRepository,
        IRoundRepository roundRepository,
        IRegisterTeamRepository registerTeamRepository,
        ITrackRepository trackRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _submissionRepository = submissionRepository;
        _eventRepository = eventRepository;
        _roundRepository = roundRepository;
        _registerTeamRepository = registerTeamRepository;
        _trackRepository = trackRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    private async Task EnsureStaffAssignedToEvent(Guid eventId)
    {
        await StaffAssignmentHelper.ValidateAndGetAssignmentAsync(
            _assignEventRepository, _currentUserService, eventId);
    }

    public async Task<GetSubmissionDetailResponse> GetDetail(Guid submissionId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var eventId = submission.RoundDetail.RegisterTeam.EventId;
        await EnsureStaffAssignedToEvent(eventId);

        var validScores = submission.Scores.Where(s => s.TotalScore.HasValue).ToList();
        var totalScore = validScores.Count > 0
            ? Math.Round(validScores.Average(s => s.TotalScore!.Value), 2)
            : (decimal?)null;

        return new GetSubmissionDetailResponse
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
                .Select(td => new SubmittedByUser
                {
                    UserId = td.UserId,
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

    public async Task<GetSubmissionsResponse> GetByEvent(Guid eventId, GetSubmissionsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);
        await EnsureStaffAssignedToEvent(eventId);

        request.EventId = eventId;
        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            request.EventId, request.RoundId, request.TrackId,
            request.TopicId, request.RegisterTeamId, request.Keyword,
            request.PageIndex, request.PageSize);

        return MapResponse(items, totalCount, request.PageIndex, request.PageSize);
    }

    public async Task<GetSubmissionsResponse> GetByRound(Guid roundId, string? keyword, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var round = await _roundRepository.GetByIdAsync(roundId);
        if (round == null)
            throw new NotFoundException("Round Not Found");

        await EnsureStaffAssignedToEvent(round.EventId);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            round.EventId, roundId, null, null, null, keyword,
            pageIndex, pageSize);

        return MapResponse(items, totalCount, pageIndex, pageSize);
    }

    public async Task<GetSubmissionsResponse> GetByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        await EnsureStaffAssignedToEvent(registerTeam.EventId);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            registerTeam.EventId, roundId, null, null, registerTeamId, null,
            pageIndex, pageSize);

        return MapResponse(items, totalCount, pageIndex, pageSize);
    }

    public async Task<GetSubmissionsResponse> GetByTrack(Guid trackId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        await EnsureStaffAssignedToEvent(track.EventId);

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            track.EventId, null, trackId, null, null, null,
            pageIndex, pageSize);

        return MapResponse(items, totalCount, pageIndex, pageSize);
    }

    private static GetSubmissionsResponse MapResponse(List<RoundDetails> items, int totalCount, int pageIndex, int pageSize)
    {
        return new GetSubmissionsResponse
        {
            Items = items.Select(rd => new SubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                EventId = rd.RegisterTeam.EventId,
                EventName = rd.RegisterTeam.Event.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                TrackId = rd.RegisterTeam.TrackId,
                TrackTitle = rd.RegisterTeam.Track?.Title,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmittedBy = rd.RegisterTeam.Team.TeamDetails
                    .Where(td => td.IsLeader)
                    .Select(td => new SubmittedByUser
                    {
                        UserId = td.UserId,
                        Email = td.User.Email,
                        FirstName = td.User.FirstName,
                        LastName = td.User.LastName
                    })
                    .FirstOrDefault(),
                LastSubmission = SubmissionHelper.GetLastSubmission(rd) is { } lastSub
                    ? new SubmissionRecordDto
                    {
                        Id = lastSub.Id,
                        SubmittedAt = lastSub.SubmittedAt,
                        Url = lastSub.Url,
                        Description = lastSub.Description,
                        Status = lastSub.Status?.ToString()
                    }
                    : null,
                Records = rd.Submissions
                    .OrderByDescending(s => s.SubmittedAt)
                    .Select(s => new SubmissionRecordDto
                    {
                        Id = s.Id,
                        SubmittedAt = s.SubmittedAt,
                        Url = s.Url,
                        Description = s.Description,
                        Status = s.Status?.ToString()
                    })
                    .ToList()
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
