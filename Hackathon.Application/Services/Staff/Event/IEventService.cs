using Hackathon.Application.Services.Admin.Event;

namespace Hackathon.Application.Services.Staff.Event;

public interface IEventService
{
    Task<GetRecentEventsResponse> GetRecentEvents();
    Task<GetEventCountResponse> GetEventCount(GetEventCountRequest request);
    Task<GetMyEventsResponse> GetMyEvents(GetMyEventsRequest request);
    Task<GetMyEventsResponse> GetMyStaffEvents(GetMyEventsRequest request);
    Task<List<StaffEventItem>> GetMyCurrentEvents();
    Task<GetMyEventDetailResponse> GetMyEventDetail(Guid eventId);
}
