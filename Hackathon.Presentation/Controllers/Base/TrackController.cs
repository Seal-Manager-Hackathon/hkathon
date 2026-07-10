using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Track;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/tracks")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
