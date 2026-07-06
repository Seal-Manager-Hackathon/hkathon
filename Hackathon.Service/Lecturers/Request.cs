using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Lecturers;

public static class Request
{
    public class SearchLecturerEventsRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public int? Year { get; set; }
        public EventRoleEnum? EventRole { get; set; }
    }
}
