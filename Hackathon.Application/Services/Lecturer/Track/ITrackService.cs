using Hackathon.Application.Services.Staff.Track;

namespace Hackathon.Application.Services.Lecturer.Track;

public interface ITrackService
{
    Task<GetTracksResponse> GetTracks(Guid eventId, GetTracksRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}
