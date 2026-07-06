namespace Hackathon.Application.Services.Event;

public interface IEventService
{
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetRecentEventsResponse> GetRecentEvents();
}
