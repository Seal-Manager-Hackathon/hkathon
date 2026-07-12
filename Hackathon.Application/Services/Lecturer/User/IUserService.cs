using Hackathon.Application.Services.Admin.User;

namespace Hackathon.Application.Services.Lecturer.User;

public interface IUserService
{
    // Existing lecturer user methods
    Task<GetRecentUsersResponse> GetRecentUsers();
    Task<GetUserCountResponse> GetUserCount(GetUserCountRequest request);
    // New — from Admin User, filter IsDisable=false
    Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request);
    Task<UserDetailResponse> GetUserDetail(Guid userId);
}
