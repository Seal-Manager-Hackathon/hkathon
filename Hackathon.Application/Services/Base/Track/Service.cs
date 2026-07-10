using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Track;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Base.Track;

public class Service : ITrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(
        ITrackRepository trackRepository,
        IAuthorizationService authorizationService)
    {
        _trackRepository = trackRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId)
    {
        _authorizationService.Authenticate();

        var track = await _trackRepository.GetByIdAsync(trackId);
        if (track == null)
            throw new NotFoundException(ErrMsg.Common.ResourceNotFound);

        return new GetTrackDetailResponse
        {
            Id = track.Id,
            EventId = track.EventId,
            Title = track.Title,
            Description = track.Description,
            MaxTeam = track.MaxTeam,
            IsDisable = track.IsDisable,
            RegisterTeamCount = 0,
            CreatedAt = track.CreatedAt,
            UpdatedAt = track.UpdatedAt
        };
    }
}
