namespace Hackathon.Application.Services.Track;

public interface ITrackService
{
    Task<CreateTrackResponse> CreateTrack(CreateTrackRequest request);
    Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid eventId, Guid trackId);
    Task UpdateTrack(UpdateTrackRequest request);
    Task DeleteTrack(Guid trackId);
    Task RestoreTrack(Guid trackId);
}
