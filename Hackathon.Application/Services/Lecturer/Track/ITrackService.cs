namespace Hackathon.Application.Services.Lecturer.Track;

public interface ITrackService
{
    Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
    Task<GetMyAssignedTracksResponse> GetMyAssignedTracks(Guid eventId, int pageIndex = 1, int pageSize = 10);
}