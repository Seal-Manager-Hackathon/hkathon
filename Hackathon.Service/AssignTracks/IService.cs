namespace Hackathon.Service.AssignTracks;

public interface IService
{
    // #{Staff} #{Admin}
    Task<Response.AssignTrackResponse> AssignLecturerToTrack(Guid eventId, Guid trackId, Request.AssignJudgeRequest request);
    Task<List<Response.AssignTrackLecturerResponse>> GetLecturersAssignedToTrack(Guid eventId, Guid trackId, bool? isDisable);
    Task<Guid> RemoveLecturerFromTrack(Guid assignTrackId);
}
