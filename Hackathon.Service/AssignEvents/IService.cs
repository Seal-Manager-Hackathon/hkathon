using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.AssignEvents;

public interface IService
{
    // #{Staff} #{Admin}
    Task<Response.AssignEventResponse> AssignLecturerToEvent(Guid eventId, Request.AssignLecturerRequest request);
    Task<BasePaginationResponse> GetAvailableLecturers(Guid eventId, Request.GetAvailableLecturersRequest request);
    Task<BasePaginationResponse> GetEventAssignments(Guid eventId, EventRoleEnum? eventRole, string? keyword, Guid? trackId, bool? isDisable, PaginationRequest paginationRequest);
    Task<Guid> RemoveLecturerAssignment(Guid assignEventId);
}
