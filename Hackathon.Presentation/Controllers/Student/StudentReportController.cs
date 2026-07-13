using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Report;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public StudentReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// POST /api/v1/student/reports — Create a new report
    /// </summary>
    [HttpPost("reports")]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportRequest request)
    {
        var result = await _reportService.CreateReport(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Report.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// GET /api/v1/student/reports — List user's reports
    /// </summary>
    [HttpGet("reports")]
    public async Task<IActionResult> GetReports([FromQuery] GetReportsRequest request)
    {
        var result = await _reportService.GetReports(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// GET /api/v1/student/reports/{reportId} — Get report detail
    /// </summary>
    [HttpGet("reports/{reportId:guid}")]
    public async Task<IActionResult> GetReportDetail(Guid reportId)
    {
        var result = await _reportService.GetReportDetail(reportId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// POST /api/v1/student/reports/{reportId}/cancel — Cancel a report (soft delete, set status = Canceled)
    /// </summary>
    [HttpPost("reports/{reportId:guid}/cancel")]
    public async Task<IActionResult> CancelReport(Guid reportId)
    {
        await _reportService.CancelReport(reportId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
