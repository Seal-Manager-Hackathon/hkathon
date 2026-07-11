using Hackathon.Application.Services.Admin.User;

namespace Hackathon.Application.Services.Lecturer.User;

public interface IUserService
{
    Task<GetRecentUsersResponse> GetRecentUsers();
    Task<GetUserCountResponse> GetUserCount(GetUserCountRequest request);
}