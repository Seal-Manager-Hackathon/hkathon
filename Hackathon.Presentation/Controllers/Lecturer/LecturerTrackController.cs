using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Track;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerTrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public LecturerTrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("tracks/{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
