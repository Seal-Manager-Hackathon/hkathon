namespace Hackathon.Application.Services.Event;

public interface IEventService
{
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetRecentEventsResponse> GetRecentEvents();
    Task<GetEventsResponse> GetEvents(GetEventsRequest request);
    Task<GetEventDetailResponse> GetEventDetail(Guid eventId);
    Task CreateEvent(CreateEventRequest request);
}
