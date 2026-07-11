using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Staff.Track;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITrackRepository trackRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService)
    {
        _trackRepository = trackRepository;
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTracksResponse> GetTracks(Guid eventId, GetTracksRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Lecturer);

        PaginationHelper.Validate(request.PageIndex, request.PageSize);

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
            Tracks = trackItems,
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
}
