using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Student.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IRegisterTeamRepository _registerTeamRepository;

    public Service(
        ITrackRepository trackRepository,
        IEventRepository eventRepository,
        IRegisterTeamRepository registerTeamRepository)
    {
        _trackRepository = trackRepository;
        _eventRepository = eventRepository;
        _registerTeamRepository = registerTeamRepository;
    }

    public async Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId)
    {
        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null || track.IsDisable)
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

    public async Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request)
    {
        var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
        if (eventEntity == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        var allTracks = await _trackRepository.GetByEventIdAsync(request.EventId);

        // Student: only see non-disabled tracks
        var query = allTracks.Where(t => !t.IsDisable).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(t => t.Title.ToLower().Contains(kw));
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
