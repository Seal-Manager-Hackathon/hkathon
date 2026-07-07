namespace Hackathon.Application.Services.Admin.Event;

public interface IEventService
{
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetRecentEventsResponse> GetRecentEvents();
    Task<GetEventsResponse> GetEvents(GetEventsRequest request);
    Task<GetEventDetailResponse> GetEventDetail(Guid eventId);
    Task CreateEvent(CreateEventRequest request);
    Task UpdateEvent(UpdateEventRequest request);
    Task<SetupCheckResponse> IsEventSetupComplete(Guid eventId);
}
