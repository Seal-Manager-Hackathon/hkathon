namespace Hackathon.Application.Services.Student.Track;

public interface ITrackService
{
    Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}
