using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Submission;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminSubmissionController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    public AdminSubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetSubmissionDetail(Guid submissionId)
    {
        var result = await _submissionService.GetSubmissionDetail(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissions(Guid eventId, [FromQuery] GetSubmissionsRequest request)
    {
        request.EventId = eventId;
        var result = await _submissionService.GetSubmissions(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("rounds/{roundId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRound(Guid roundId, [FromQuery] string? keyword, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetSubmissionsByRound(roundId, keyword, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRegisterTeam(Guid registerTeamId, [FromQuery] Guid? roundId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetSubmissionsByRegisterTeam(registerTeamId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByTrack(Guid trackId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetSubmissionsByTrack(trackId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
