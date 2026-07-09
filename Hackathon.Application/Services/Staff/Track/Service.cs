using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IAssignEventRepository _assignEventRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITrackRepository trackRepository,
        IAssignEventRepository assignEventRepository,
        IRegisterTeamRepository registerTeamRepository,
        ICurrentUserService currentUserService,
        IAuthorizationService authorizationService)
    {
        _trackRepository = trackRepository;
        _assignEventRepository = assignEventRepository;
        _registerTeamRepository = registerTeamRepository;
        _currentUserService = currentUserService;
        _authorizationService = authorizationService;
    }

    public async Task<GetTracksResponse> GetTracks(Guid eventId, GetTracksRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            eventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

        var (items, totalCount) = await _trackRepository.GetByEventIdAsync(
            eventId, request.Keyword, isDisable: false,
            request.PageIndex, request.PageSize);

        var trackItems = items
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new StaffTrackItem
            {
                Id = t.Id,
                EventId = t.EventId,
                Title = t.Title,
                Description = t.Description,
                MaxTeam = t.MaxTeam,
                IsDisable = t.IsDisable,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return new GetTracksResponse
        {
            Items = trackItems,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }

    public async Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Staff);

        var currentUserId = _currentUserService.UserId;
        if (!currentUserId.HasValue)
            throw new UnauthorizedException(ErrMsg.Auth.InvalidOrExpiredToken);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
            throw new NotFoundException("Track Not Found");

        var assignEvent = await _assignEventRepository.GetByEventIdAndUserIdAsync(
            track.EventId, currentUserId.Value);
        if (assignEvent == null)
            throw new ForbiddenException("You Are Not Assigned to This Event");

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
}
