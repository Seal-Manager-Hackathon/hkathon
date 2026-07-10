namespace Hackathon.Application.Services.Staff.Assign;

public interface IAssignService
{
    Task<GetAvailableStaffResponse> GetAvailableStaff(Guid eventId, GetAvailableLecturersRequest request);
    Task<GetAvailableLecturersResponse> GetAvailableLecturers(Guid eventId, GetAvailableLecturersRequest request);
    Task AssignLecturerToEvent(Guid eventId, AssignLecturerToEventRequest request);
    Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request);
    Task<GetAssignEventDetailResponse> GetAssignEventDetail(Guid assignEventId);
    Task RemoveAssignEvent(Guid assignEventId);
    Task RestoreAssignEvent(Guid assignEventId);
    Task AssignTrackToEvent(Guid assignEventId, Guid trackId);
    Task RemoveTrackFromEvent(Guid assignEventId, Guid trackId);
    Task RestoreTrackToEvent(Guid assignEventId, Guid trackId);
}
