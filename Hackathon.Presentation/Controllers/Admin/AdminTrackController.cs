using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Track;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminTrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public AdminTrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpPost("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> CreateTrack(Guid eventId, [FromBody] CreateTrackRequest request)
    {
        request.EventId = eventId;
        var result = await _trackService.CreateTrack(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetTracksByEvent(Guid eventId, [FromQuery] GetTracksByEventRequest request)
    {
        request.EventId = eventId;
        var result = await _trackService.GetTracksByEvent(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/tracks/{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid eventId, Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(eventId, trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPut("events/{eventId:guid}/tracks/{trackId:guid}")]
    public async Task<IActionResult> UpdateTrack(Guid eventId, Guid trackId, [FromBody] UpdateTrackRequest request)
    {
        request.TrackId = trackId;
        await _trackService.UpdateTrack(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("tracks/{trackId:guid}/delete")]
    public async Task<IActionResult> DeleteTrack(Guid trackId)
    {
        await _trackService.DeleteTrack(trackId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("tracks/{trackId:guid}/restore")]
    public async Task<IActionResult> RestoreTrack(Guid trackId)
    {
        await _trackService.RestoreTrack(trackId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }
}
