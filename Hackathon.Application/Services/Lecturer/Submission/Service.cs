using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Submission;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Submission;

public class Service : ISubmissionService
{
    private readonly ISubmissionRepository _submissionRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ISubmissionRepository submissionRepository,
        IEventRepository eventRepository,
        IRegisterTeamRepository registerTeamRepository,
        ITrackRepository trackRepository,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _submissionRepository = submissionRepository;
        _eventRepository = eventRepository;
        _registerTeamRepository = registerTeamRepository;
        _trackRepository = trackRepository;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetLecturerSubmissionsResponse> GetSubmissions(GetLecturerSubmissionsRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var ev = await _eventRepository.GetByIdAsync(request.EventId);
        if (ev == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Check lecturer is assigned to this event
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(request.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            request.EventId, request.RoundId, request.TrackId,
            request.TopicId, request.RegisterTeamId, request.Keyword,
            request.PageIndex, request.PageSize);

        return new GetLecturerSubmissionsResponse
        {
            Items = items.Select(rd => new LecturerSubmissionItem
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
                    : null
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetLecturerSubmissionsResponse> GetSubmissionsByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var registerTeam = await _registerTeamRepository.GetByIdAsync(registerTeamId);
        if (registerTeam == null)
            throw new NotFoundException("Register Team Not Found");

        // Check lecturer is assigned to this event
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(registerTeam.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            registerTeam.EventId, roundId, null, null, registerTeamId, null,
            pageIndex, pageSize);

        return new GetLecturerSubmissionsResponse
        {
            Items = items.Select(rd => new LecturerSubmissionItem
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
                    : null
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetLecturerSubmissionsResponse> GetSubmissionsByTrack(Guid trackId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException("Track Not Found");

        // Check lecturer is assigned to this event
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            track.EventId, null, trackId, null, null, null,
            pageIndex, pageSize);

        return new GetLecturerSubmissionsResponse
        {
            Items = items.Select(rd => new LecturerSubmissionItem
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
                    : null
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    public async Task<GetSubmissionDetailResponse> GetSubmissionDetail(Guid submissionId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var submission = await _submissionRepository.GetByIdAsync(submissionId);
        if (submission == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        // Check lecturer is assigned to this event
        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var registerTeam = submission.RoundDetail?.RegisterTeam;
        if (registerTeam != null)
        {
            var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(registerTeam.EventId, currentUserId.Value);
            if (assignEvent == null)
                throw new ForbiddenException("You Are Not Assigned to This Event");
        }

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
                .Select(td => new Admin.Submission.SubmittedByUser
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
}
