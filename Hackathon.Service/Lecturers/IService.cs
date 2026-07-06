using Hackathon.Service.Models;

namespace Hackathon.Service.Lecturers;

public interface IService
{
    // #{Lecturer}
    Task<BasePaginationResponse> GetLecturerEvents(PaginationRequest request);
    Task<BasePaginationResponse> SearchLecturerEvents(Request.SearchLecturerEventsRequest request);
    Task<List<Response.LecturerEventResponse>> GetCurrentLecturerEvents();
    Task<Response.LecturerEventTracksResponse> GetLecturerTracks(Guid eventId);
}
