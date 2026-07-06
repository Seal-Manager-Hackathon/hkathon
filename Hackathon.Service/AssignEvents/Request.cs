using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.AssignEvents;

public static class Request
{
    public class AssignLecturerRequest
    {
        public Guid LecturerId { get; set; }
        public EventRoleEnum EventRole { get; set; }
    }

    public class GetAvailableLecturersRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public Guid? UserId { get; set; }
    }
}
