using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUnitOfWork _unitOfWork;

    public Service(
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        IRegisterTeamRepository registerTeamRepository,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork)
    {
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _registerTeamRepository = registerTeamRepository;
        _authorizationService = authorizationService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateTrackResponse> CreateTrack(CreateTrackRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var now = DateTimeOffset.UtcNow;
        var track = new Tracks
        {
            Id = Guid.NewGuid(),
            EventId = request.EventId,
            Title = request.Title,
            Description = request.Description,
            MaxTeam = request.MaxTeam,
            IsDisable = false,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _trackRepository.AddAsync(track);
        await _unitOfWork.SaveChangesAsync();

        return new CreateTrackResponse
        {
            Id = track.Id,
            EventId = track.EventId,
            Title = track.Title,
            Description = track.Description,
            MaxTeam = track.MaxTeam,
            IsDisable = track.IsDisable
        };
    }

    public async Task<GetTrackDetailResponse> GetTrackDetail(Guid eventId, Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(eventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.EventId != eventId)
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

    public async Task UpdateTrack(UpdateTrackRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var track = await _trackRepository.GetByIdAsync(request.TrackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        if (request.Title != null)
            track.Title = request.Title;
        if (request.Description != null)
            track.Description = request.Description;
        if (request.MaxTeam.HasValue)
            track.MaxTeam = request.MaxTeam.Value;
        if (request.IsDisable.HasValue)
            track.IsDisable = request.IsDisable.Value;

        track.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteTrack(Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        track.IsDisable = true;
        track.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreTrack(Guid trackId)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        track.IsDisable = false;
        track.UpdatedAt = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request)
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allTracks = await _trackRepository.GetByEventIdAsync(request.EventId);

        // Filter in-memory after materialization
        var query = allTracks.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
        }

        if (request.IsDisable.HasValue)
        {
            query = query.Where(t => t.IsDisable == request.IsDisable.Value);
        }

        var totalCount = query.Count();

        var items = query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(t => new TrackItem
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

        return new GetTracksByEventResponse
        {
            Tracks = items,
            TotalCount = totalCount,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
