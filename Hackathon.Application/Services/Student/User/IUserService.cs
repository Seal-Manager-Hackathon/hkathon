namespace Hackathon.Application.Services.Student.User;

public interface IUserService
{
    Task<StudentUserDetailResponse> GetUserDetail(Guid userId);
}
