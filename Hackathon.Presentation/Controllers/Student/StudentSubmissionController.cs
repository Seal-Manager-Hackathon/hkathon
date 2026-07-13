using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Submission;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentSubmissionController : ControllerBase
{
    private readonly ISubmissionService _submissionService;

    public StudentSubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    /// <summary>
    /// GET /api/v1/student/register-teams/{registerTeamId}/submissions
    /// Get all submissions (last per round) for a register team, optionally filtered by roundId
    /// </summary>
    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetRegisterTeamSubmissions(Guid registerTeamId, [FromQuery] Guid? roundId)
    {
        var result = await _submissionService.GetRegisterTeamSubmissions(registerTeamId, roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// GET /api/v1/student/submissions/{submissionId}
    /// Get submission detail
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetSubmissionDetail(Guid submissionId)
    {
        var result = await _submissionService.GetSubmissionDetail(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
