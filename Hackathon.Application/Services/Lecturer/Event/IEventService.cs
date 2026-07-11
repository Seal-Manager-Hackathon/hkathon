using Hackathon.Application.Services.Admin.Event;

namespace Hackathon.Application.Services.Lecturer.Event;

public interface IEventService
{
    Task<GetLecturerEventsResponse> GetLecturerEvents(GetLecturerEventsRequest request);
    Task<GetLecturerEventsResponse> GetLecturerAssignEvents(GetLecturerEventsRequest request);
    Task<GetRecentEventsResponse> GetRecentEvents();
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetLecturerEventDetailResponse> GetLecturerEventDetail(Guid eventId);
}