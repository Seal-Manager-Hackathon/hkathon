namespace Hackathon.Application.Services.Staff.User;

public interface IUserService
{
    Task<GetAllUsersResponse> GetAllUsers(GetAllUsersRequest request);
    Task<UserDetailResponse> GetUserDetail(GetUserDetailRequest request);
}