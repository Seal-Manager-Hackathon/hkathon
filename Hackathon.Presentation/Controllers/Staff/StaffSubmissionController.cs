using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Submission;
using Microsoft.AspNetCore.Mvc;
using StaffSubmissionService = Hackathon.Application.Services.Staff.Submission.ISubmissionService;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffSubmissionController : ControllerBase
{
    private readonly StaffSubmissionService _submissionService;

    public StaffSubmissionController(StaffSubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetSubmissionDetail(Guid submissionId)
    {
        var result = await _submissionService.GetDetail(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissions(Guid eventId, [FromQuery] GetSubmissionsRequest request)
    {
        var result = await _submissionService.GetByEvent(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("rounds/{roundId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRound(Guid roundId, [FromQuery] string? keyword, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetByRound(roundId, keyword, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRegisterTeam(Guid registerTeamId, [FromQuery] Guid? roundId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetByRegisterTeam(registerTeamId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByTrack(Guid trackId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetByTrack(trackId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
