using Hackathon.Service.Models;

namespace Hackathon.Service.Mentors;

public static class Request
{
    public class GetMentorEventsRequest : PaginationRequest
    {
    }

    public class SendNotificationRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
