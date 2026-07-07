using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminRoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public AdminRoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpGet("events/{eventId:guid}/rounds")]
    public async Task<IActionResult> GetRounds(Guid eventId, [FromQuery] GetRoundsRequest request)
    {
        request.EventId = eventId;
        var result = await _roundService.GetRounds(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RoundsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events/{eventId:guid}/rounds")]
    public async Task<IActionResult> CreateRound(Guid eventId, [FromBody] CreateRoundRequest request)
    {
        request.EventId = eventId;
        await _roundService.CreateRound(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.RoundCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }
}
