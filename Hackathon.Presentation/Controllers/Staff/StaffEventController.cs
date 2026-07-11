using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Event;
using Microsoft.AspNetCore.Mvc;
using StaffEventService = Hackathon.Application.Services.Staff.Event.IEventService;
using StaffModels = Hackathon.Application.Services.Staff.Event;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffEventController : ControllerBase
{
    private readonly StaffEventService _eventService;

    public StaffEventController(StaffEventService eventService)
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
    public async Task<IActionResult> GetMyEvents([FromQuery] StaffModels.GetMyEventsRequest request)
    {
        var result = await _eventService.GetMyEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/my-staff")]
    public async Task<IActionResult> GetMyStaffEvents([FromQuery] StaffModels.GetMyEventsRequest request)
    {
        var result = await _eventService.GetMyStaffEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/current")]
    public async Task<IActionResult> GetMyCurrentEvents()
    {
        var result = await _eventService.GetMyCurrentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetMyEventDetail(Guid eventId)
    {
        var result = await _eventService.GetMyEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }
}