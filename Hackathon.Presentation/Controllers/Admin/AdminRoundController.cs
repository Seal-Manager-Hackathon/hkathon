using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Round;
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

    [HttpPatch("rounds/{roundId:guid}")]
    public async Task<IActionResult> UpdateRound(Guid roundId, [FromBody] UpdateRoundRequest request)
    {
        request.RoundId = roundId;
        await _roundService.UpdateRound(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.RoundUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("events/{eventId:guid}/rounds/{roundId:guid}/swap")]
    public async Task<IActionResult> SwapRound(Guid eventId, Guid roundId, [FromBody] SwapRoundRequest request)
    {
        request.EventId = eventId;
        request.RoundId = roundId;
        await _roundService.SwapRound(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/rounds/max-round-no")]
    public async Task<IActionResult> GetMaxRoundNo(Guid eventId)
    {
        var result = await _roundService.GetMaxRoundNo(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("rounds/{roundId:guid}/delete")]
    public async Task<IActionResult> DeleteRound(Guid roundId)
    {
        await _roundService.DeleteRound(roundId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("rounds/{roundId:guid}/restore")]
    public async Task<IActionResult> RestoreRound(Guid roundId)
    {
        await _roundService.RestoreRound(roundId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }
}
