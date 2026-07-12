using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Event;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentEventController : ControllerBase
{
    private readonly IEventService _eventService;

    public StudentEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request)
    {
        var result = await _eventService.GetEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetEventDetail(Guid eventId)
    {
        var result = await _eventService.GetEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/count")]
    public async Task<IActionResult> GetEventCount([FromQuery] GetEventCountRequest request)
    {
        var result = await _eventService.GetEventCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        var result = await _eventService.GetRecentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
