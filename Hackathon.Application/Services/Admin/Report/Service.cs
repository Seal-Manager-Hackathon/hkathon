using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.Admin.Report;

public class Service : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IAuthorizationService _authorizationService;

    public Service(IReportRepository reportRepository, IAuthorizationService authorizationService)
    {
        _reportRepository = reportRepository;
        _authorizationService = authorizationService;
    }

    public async Task<GetRecentReportsResponse> GetRecentReports()
    {
        _authorizationService.Authorize(RoleEnum.Admin);

        var reports = await _reportRepository.GetRecentAsync(10);

        return new GetRecentReportsResponse
        {
            Reports = reports.Select(r => new ReportItem
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                Status = r.Status?.ToString(),
                TypeReport = r.TypeReport,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}
