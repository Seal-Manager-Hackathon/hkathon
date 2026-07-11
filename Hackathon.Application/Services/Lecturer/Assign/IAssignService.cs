namespace Hackathon.Application.Services.Lecturer.Assign;

public interface IAssignService
{
    Task<GetAssignedUsersResponse> GetAssignedUsers(Guid eventId, GetAssignedUsersRequest request);
    Task<GetLecturerAssignedInfoResponse> GetLecturerAssignedInfo(Guid eventId);
    Task<GetAvailableUserResponse> GetAvailableStaff(GetAvailableUserRequest request);
    Task<GetAvailableUserResponse> GetAvailableLecturer(GetAvailableUserRequest request);
}
