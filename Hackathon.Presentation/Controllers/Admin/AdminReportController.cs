using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public AdminReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("reports/recent")]
    public async Task<IActionResult> GetRecentReports()
    {
        var result = await _reportService.GetRecentReports();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentReportsFetched, traceId: HttpContext.TraceIdentifier));
    }
}
