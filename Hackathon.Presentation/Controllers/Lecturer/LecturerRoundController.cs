using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerRoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public LecturerRoundController(IRoundService roundService)
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

    [HttpGet("events/{eventId:guid}/rounds/max-round-no")]
    public async Task<IActionResult> GetMaxRoundNo(Guid eventId)
    {
        var result = await _roundService.GetMaxRoundNo(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("rounds/{roundId:guid}")]
    public async Task<IActionResult> GetRoundDetail(Guid roundId)
    {
        var result = await _roundService.GetRoundDetail(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}