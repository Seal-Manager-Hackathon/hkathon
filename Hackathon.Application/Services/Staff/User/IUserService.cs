using Hackathon.Application.Services.Admin.User;

namespace Hackathon.Application.Services.Staff.User;

public interface IUserService
{
    Task<GetRecentUsersResponse> GetRecentUsers();
    Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request);
    Task<UserDetailResponse> GetUserDetail(GetUserDetailRequest request);
}