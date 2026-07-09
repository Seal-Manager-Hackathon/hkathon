namespace Hackathon.Application.Services.Staff.Assign;

public interface IAssignService
{
    Task<GetAvailableLecturersResponse> GetAvailableLecturers(Guid eventId, GetAvailableLecturersRequest request);
    Task AssignLecturerToEvent(Guid eventId, AssignLecturerToEventRequest request);
    Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request);
    Task<GetAssignEventDetailResponse> GetAssignEventDetail(Guid eventId, Guid assignEventId);
}
