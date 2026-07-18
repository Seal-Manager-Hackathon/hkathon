namespace Hackathon.Application.Services.Student.PopularEvent;

public interface IPopularEventService
{
    Task<GetPopularEventsResponse> GetPopularEvents(GetPopularEventsRequest request);
}
