using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Event;
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

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request)
    {
        var result = await _eventService.GetEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        await _eventService.CreateEvent(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.EventCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        var result = await _eventService.GetRecentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentEventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetEventDetail(Guid eventId)
    {
        var result = await _eventService.GetEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("events/{eventId:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid eventId, [FromBody] UpdateEventRequest request)
    {
        request.EventId = eventId;
        await _eventService.UpdateEvent(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.EventUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/count")]
    public async Task<IActionResult> GetEventCount([FromQuery] GetEventCountRequest request)
    {
        var result = await _eventService.GetEventCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventCountFetched, traceId: HttpContext.TraceIdentifier));
    }
}
