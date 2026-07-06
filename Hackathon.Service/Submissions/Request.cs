using Hackathon.Service.Models;

namespace Hackathon.Service.Submissions;

public static class Request
{
    public class SubmitRoundProjectRequest
    {
        public required string Url { get; set; }

        public string? Description { get; set; }
    }

        public class GetSubmissionsRequest : PaginationRequest
    {
    }
}
