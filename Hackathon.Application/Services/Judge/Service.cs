using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.EventRole;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Judge;

public class Service : IJudgeService
{
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ISubmissionRepository _submissionRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        IAssignEventRepository assignEventRepository,
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        ISubmissionRepository submissionRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _assignEventRepository = assignEventRepository;
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _submissionRepository = submissionRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Lấy danh sách track mà judge được assign trong 1 event.
    /// Check: user phải có EventRole = Judge và được assign vào event đó.
    /// </summary>
    public async Task<List<JudgeTrackItem>> GetMyTracks(Guid eventId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Check judge được assign vào event với EventRole = Judge
        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        // Verify EventRole là Judge
        if (assignEvent.EventRole == null || assignEvent.EventRole.Name != EventRoleEnum.Judge)
            throw new ForbiddenException("You Are Not Assigned as Judge for This Event");

        var ev = await _eventRepository.GetByIdAsync(eventId);

        return assignEvent.AssignTracks
            .Where(at => !at.IsDisable && !at.Track.IsDisable)
            .Select(at => new JudgeTrackItem
            {
                AssignTrackId = at.Id,
                TrackId = at.TrackId,
                TrackTitle = at.Track.Title,
                TrackDescription = at.Track.Description,
                EventId = eventId,
                EventName = ev?.Name ?? ""
            }).ToList();
    }

    /// <summary>
    /// Lấy danh sách submissions trong 1 track mà judge được assign.
    /// Check judge có assign track này không.
    /// </summary>
    public async Task<GetTrackSubmissionsResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, int pageIndex, int pageSize)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        // Verify track exists
        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
            throw new NotFoundException("Track Not Found");

        // Verify judge is assigned to this track with EventRole = Judge
        var assignTrack = await _assignEventRepository.GetGraderAssignTrackAsync(
            currentUserId.Value, track.EventId, trackId);
        if (assignTrack == null)
            throw new ForbiddenException("You Are Not Assigned as Judge for This Track");

        PaginationHelper.Validate(pageIndex, pageSize);

        // Get submissions
        var (items, totalCount) = await _submissionRepository.GetSubmissionsAsync(
            track.EventId, roundId, trackId, null, null, null,
            pageIndex, pageSize);

        return new GetTrackSubmissionsResponse
        {
            Items = items.Select(rd => new TrackSubmissionItem
            {
                RegisterTeamId = rd.RegisterTeamId,
                TeamId = rd.RegisterTeam.TeamId,
                TeamName = rd.RegisterTeam.Team.Name,
                RoundId = rd.RoundId,
                RoundName = rd.Round.Name,
                RoundNo = rd.Round.RoundNo,
                TopicId = rd.RegisterTeam.TopicId,
                TopicTitle = rd.RegisterTeam.Topic?.Title,
                SubmissionId = SubmissionHelper.GetLastSubmission(rd)?.Id,
                Url = SubmissionHelper.GetLastSubmission(rd)?.Url,
                Description = SubmissionHelper.GetLastSubmission(rd)?.Description,
                Status = SubmissionHelper.GetLastSubmission(rd)?.Status?.ToString(),
                SubmittedAt = SubmissionHelper.GetLastSubmission(rd)?.SubmittedAt,
                GradingStatus = "Pending",
                ScoreId = null,
                TotalScore = null
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}
