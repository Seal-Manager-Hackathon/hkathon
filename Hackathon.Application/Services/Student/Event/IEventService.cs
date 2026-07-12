namespace Hackathon.Application.Services.Student.Event;

public interface IEventService
{
    Task<GetEventsResponse> GetEvents(GetEventsRequest request);
    Task<GetEventDetailResponse> GetEventDetail(Guid eventId);
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetRecentEventsResponse> GetRecentEvents();
}
