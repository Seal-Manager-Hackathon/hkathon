using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using LecturerTrack = Hackathon.Application.Services.Lecturer.Track;
using StaffTrack = Hackathon.Application.Services.Staff.Track;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerTrackController : ControllerBase
{
    private readonly LecturerTrack.ITrackService _trackService;

    public LecturerTrackController(LecturerTrack.ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetTracks(Guid eventId, [FromQuery] StaffTrack.GetTracksRequest request)
    {
        var result = await _trackService.GetTracks(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
