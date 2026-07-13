namespace Hackathon.Application.Services.Student.Report;

public interface IReportService
{
    Task<CreateReportResponse> CreateReport(CreateReportRequest request);
    Task<GetReportsResponse> GetReports(GetReportsRequest request);
    Task<GetReportDetailResponse> GetReportDetail(Guid reportId);
    Task CancelReport(Guid reportId);
}
