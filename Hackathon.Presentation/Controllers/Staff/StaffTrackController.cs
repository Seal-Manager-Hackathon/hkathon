using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Track;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffTrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public StaffTrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetTracks(Guid eventId, [FromQuery] GetTracksRequest request)
    {
        var result = await _trackService.GetTracks(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/tracks/{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}