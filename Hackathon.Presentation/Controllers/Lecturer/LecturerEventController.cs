using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Event;
using Microsoft.AspNetCore.Mvc;
using LecturerEvent = Hackathon.Application.Services.Lecturer.Event;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerEventController : ControllerBase
{
    private readonly LecturerEvent.IEventService _eventService;

    public LecturerEventController(LecturerEvent.IEventService eventService)
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

    [HttpGet("events")]
    public async Task<IActionResult> GetLecturerEvents([FromQuery] LecturerEvent.GetLecturerEventsRequest request)
    {
        var result = await _eventService.GetLecturerEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/my-lecturer")]
    public async Task<IActionResult> GetLecturerAssignEvents([FromQuery] LecturerEvent.GetLecturerEventsRequest request)
    {
        var result = await _eventService.GetLecturerAssignEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetLecturerEventDetail(Guid eventId)
    {
        var result = await _eventService.GetLecturerEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}