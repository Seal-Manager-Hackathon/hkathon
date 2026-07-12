using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentRoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public StudentRoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpGet("events/{eventId:guid}/rounds")]
    public async Task<IActionResult> GetRounds(Guid eventId, [FromQuery] GetRoundsRequest request)
    {
        request.EventId = eventId;
        var result = await _roundService.GetRounds(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("rounds/{roundId:guid}")]
    public async Task<IActionResult> GetRoundDetail(Guid roundId)
    {
        var result = await _roundService.GetRoundDetail(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
