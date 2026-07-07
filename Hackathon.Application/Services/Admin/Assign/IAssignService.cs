namespace Hackathon.Application.Services.Admin.Assign;

public interface IAssignService
{
    Task<GetAvailableUserResponse> GetAvailableStaff(GetAvailableUserRequest request);
    Task<GetAvailableUserResponse> GetAvailableLecturer(GetAvailableUserRequest request);
    Task<GetAssignedUsersResponse> GetAssignedUsers(GetAssignedUsersRequest request);
    Task AssignUserToEvent(AssignUserToEventRequest request);
    Task<GetAllAssignedUsersResponse> GetAllAssignedUsers(GetAllAssignedUsersRequest request);
    Task AssignEventRoleToLecturer(AssignEventRoleToLecturerRequest request);
}
