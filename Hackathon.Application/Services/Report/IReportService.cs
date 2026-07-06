namespace Hackathon.Application.Services.Report;

public interface IReportService
{
    Task<GetRecentReportsResponse> GetRecentReports();
}
