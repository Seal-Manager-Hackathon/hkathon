using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Event;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffEventController : ControllerBase
{
    private readonly IEventService _eventService;

    public StaffEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetMyEvents([FromQuery] GetMyEventsRequest request)
    {
        var result = await _eventService.GetMyEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/my-staff")]
    public async Task<IActionResult> GetMyStaffEvents([FromQuery] GetMyEventsRequest request)
    {
        var result = await _eventService.GetMyStaffEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetMyEventDetail(Guid eventId)
    {
        var result = await _eventService.GetMyEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }
}
