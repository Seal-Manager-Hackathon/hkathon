using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Judge;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Judge;

[Route("api/v1/judge")]
[ApiController]
public class JudgeController : ControllerBase
{
    private readonly IJudgeService _judgeService;

    public JudgeController(IJudgeService judgeService)
    {
        _judgeService = judgeService;
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetMyTracks(Guid eventId)
    {
        var result = await _judgeService.GetMyTracks(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/submissions")]
    public async Task<IActionResult> GetTrackSubmissions(
        Guid trackId,
        [FromQuery] Guid? roundId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetTrackSubmissions(trackId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
