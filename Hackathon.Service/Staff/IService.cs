using Hackathon.Service.Models;

namespace Hackathon.Service.Staff;

public interface IService
{
    // #{Staff}
    Task<List<Response.StaffEventResponse>> GetCurrentStaffEvents();
    Task<BasePaginationResponse> GetStaffEvents(PaginationRequest request);
    Task<BasePaginationResponse> SearchStaffEvents(Request.SearchStaffEventsRequest request);
    Task<BasePaginationResponse> GetReports(Request.GetStaffReportsRequest request);
    Task<Response.StaffReportDetailResponse> GetReportDetail(Guid reportId);
    Task<Response.ApproveRegradeResponse> ApproveRegrade(Guid reportId);
    Task UpdateReportStatus(Guid reportId, Request.UpdateReportStatusRequest request);
    Task<BasePaginationResponse> GetRegradeSubmissions(Request.GetRegradeSubmissionsRequest request);
    Task<string> ChangeUserRole(Guid userId, Request.StaffChangeUserRoleRequest request);
}
