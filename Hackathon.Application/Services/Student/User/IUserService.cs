namespace Hackathon.Application.Services.Student.User;

public interface IUserService
{
    Task<StudentUserDetailResponse> GetUserDetail(Guid userId);
    Task<SearchUsersResponse> SearchUsers(string? keyword, int pageIndex, int pageSize);
}
