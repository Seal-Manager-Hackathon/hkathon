using Hackathon.Service.Models;

namespace Hackathon.Service.Users;

public interface IService
{
    Task<Reponse.UserDetailResponse> GetUserById(Guid userId);
    Task<BasePaginationResponse> SearchStudents(Request.SearchStudentsRequest request);
    Task<Reponse.UserProfileDetailResponse> GetProfileUser();
    Task<string> UpdateProfile(Request.UpdateProfileRequest request);
    Task<string> CreateSystemReport(Request.CreateSystemReportRequest request);
    Task<List<Reponse.MyAssignmentResponse>> GetMyAssignments();
    Task<BasePaginationResponse> GetMyReports(Request.GetMyReportsRequest request);
    Task<Reponse.MyReportDetailResponse> GetMyReportById(Guid reportId);
    Task<string> UpdateAvatar(Request.UpdateAvatarRequest request);
}
