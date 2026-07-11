using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly ICurrentUserService _currentUserService;

    public Service(
        ITrackRepository trackRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService,
        IAssignEventRepository assignEventRepository,
        ICurrentUserService currentUserService)
    {
        _trackRepository = trackRepository;
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
        _assignEventRepository = assignEventRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

        // Lecturer: luôn lấy IsDisable = false
        var (items, totalCount) = await _trackRepository.GetByEventIdAsync(
            request.EventId, request.Keyword, isDisable: false,
            request.PageIndex, request.PageSize);

        return new GetTracksByEventResponse
        {
            Tracks = items.Select(t => new TrackItem
            {
                Id = t.Id,
                EventId = t.EventId,
                Title = t.Title,
                Description = t.Description,
                MaxTeam = t.MaxTeam,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            }).ToList(),
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var registerTeamCount = await _registerTeamRepository.CountByTrackIdAsync(trackId);

        return new GetTrackDetailResponse
        {
            Id = track.Id,
            EventId = track.EventId,
            Title = track.Title,
            Description = track.Description,
            MaxTeam = track.MaxTeam,
            IsDisable = track.IsDisable,
            RegisterTeamCount = registerTeamCount,
            CreatedAt = track.CreatedAt,
            UpdatedAt = track.UpdatedAt
        };
    }

    public async Task<GetMyAssignedTracksResponse> GetMyAssignedTracks(Guid eventId, int pageIndex = 1, int pageSize = 10)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(pageIndex, pageSize);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdWithTracksAsync(eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new NotFoundException("Event Not Found or You Are Not Assigned to This Event");

        var allTracks = assignEvent.AssignTracks
            .Where(at => !at.IsDisable && at.Track != null && !at.Track.IsDisable)
            .Select(at => new MyAssignedTrackItem
            {
                Id = at.TrackId,
                EventId = eventId,
                Title = at.Track.Title,
                Description = at.Track.Description,
                MaxTeam = at.Track.MaxTeam,
                IsDisable = false,
                EventRoleId = assignEvent.EventRoleId ?? Guid.Empty,
                EventRoleName = assignEvent.EventRole?.Name.ToString() ?? "",
                CreatedAt = at.Track.CreatedAt,
                UpdatedAt = at.Track.UpdatedAt
            })
            .ToList();

        var totalCount = allTracks.Count;
        var paged = allTracks
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new GetMyAssignedTracksResponse
        {
            Tracks = paged,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }
}