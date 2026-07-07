namespace Hackathon.Application.Services.Admin.Report;

public interface IReportService
{
    Task<GetRecentReportsResponse> GetRecentReports();
    Task<GetReportsResponse> GetReports(GetReportsRequest request);
    Task<GetReportDetailResponse> GetReportDetail(Guid reportId);
}
