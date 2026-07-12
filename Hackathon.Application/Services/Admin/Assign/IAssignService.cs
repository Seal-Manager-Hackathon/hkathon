namespace Hackathon.Application.Services.Admin.Assign;

public interface IAssignService
{
    Task<GetAvailableUserResponse> GetAvailableStaff(GetAvailableUserRequest request);
    Task<GetAvailableUserResponse> GetAvailableLecturer(GetAvailableUserRequest request);
    Task<GetAssignedUsersResponse> GetAssignedUsers(GetAssignedUsersRequest request);
    Task AssignUserToEvent(AssignUserToEventRequest request);
    Task AssignEventRoleToLecturer(AssignEventRoleToLecturerRequest request);
    Task AssignTrackToEvent(AssignTrackToEventRequest request);
    Task RemoveTrackFromEvent(Guid assignEventId, Guid trackId);
    Task RestoreTrackToEvent(Guid assignEventId, Guid trackId);
    Task RemoveAssignEvent(Guid assignEventId);
    Task RestoreAssignEvent(Guid assignEventId);
    Task<GetUserAssignEventsResponse> GetUserAssignEvents(Guid userId, string? keyword, string? eventRole, int pageIndex, int pageSize);
}
