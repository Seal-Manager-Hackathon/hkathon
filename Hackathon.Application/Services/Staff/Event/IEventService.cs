namespace Hackathon.Application.Services.Staff.Event;

public interface IEventService
{
    Task<GetMyEventsResponse> GetMyEvents(GetMyEventsRequest request);
    Task<GetMyEventsResponse> GetMyStaffEvents(GetMyEventsRequest request);
    Task<GetMyEventDetailResponse> GetMyEventDetail(Guid eventId);
}
