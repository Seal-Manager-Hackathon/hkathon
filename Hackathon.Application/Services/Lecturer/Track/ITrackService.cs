namespace Hackathon.Application.Services.Lecturer.Track;

public interface ITrackService
{
    Task<GetTracksByEventResponse> GetTracksByEvent(GetTracksByEventRequest request);
    Task<GetTrackDetailResponse> GetTrackDetail(Guid trackId);
}