namespace Hackathon.Application.Services.Staff.Report;

public interface IReportService
{
    Task<GetRecentReportsResponse> GetRecentReports();
    Task<GetReportsResponse> GetReports(GetReportsRequest request);
    Task<GetReportDetailResponse> GetReportDetail(Guid reportId);
    Task UpdateReportStatus(Guid reportId, UpdateReportStatusRequest request);
}
