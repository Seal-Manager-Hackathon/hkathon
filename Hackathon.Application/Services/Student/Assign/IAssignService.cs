namespace Hackathon.Application.Services.Student.Assign;

public interface IAssignService
{
    Task<GetAssignedUsersResponse> GetAssignedUsers(GetAssignedUsersRequest request);
}
