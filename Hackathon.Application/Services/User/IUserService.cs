namespace Hackathon.Application.Services.User;

public interface IUserService
{
    Task<GetRecentUsersResponse> GetRecentUsers();
    Task<CreateUserResponse> CreateUser(CreateUserRequest request);
    Task<GetUserCountResponse> GetUserCount(GetUserCountRequest request);
    Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request);
    Task<UserDetailResponse> GetUserDetail(GetUserDetailRequest request);
    Task<GetMyProfileResponse> GetMyProfile();
    Task UpdateUser(UpdateUserRequest request);
    Task DeleteUser(Guid userId);
    Task RestoreUser(Guid userId);
    Task BanUser(BanUserRequest request);
    Task UnbanUser(Guid userId);
}
