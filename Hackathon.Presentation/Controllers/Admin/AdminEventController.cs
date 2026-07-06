using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Event;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminEventController : ControllerBase
{
    private readonly IEventService _eventService;

    public AdminEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("events/recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        var result = await _eventService.GetRecentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentEventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/count")]
    public async Task<IActionResult> GetEventCount([FromQuery] GetEventCountRequest request)
    {
        var result = await _eventService.GetEventCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventCountFetched, traceId: HttpContext.TraceIdentifier));
    }
}
