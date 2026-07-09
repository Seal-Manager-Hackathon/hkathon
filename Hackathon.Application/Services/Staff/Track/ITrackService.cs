namespace Hackathon.Application.Services.Staff.Track;

public interface ITrackService
{
    Task<GetTracksResponse> GetTracks(Guid eventId, GetTracksRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}
