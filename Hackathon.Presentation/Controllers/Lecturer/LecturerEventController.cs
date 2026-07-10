using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Event;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerEventController : ControllerBase
{
    private readonly IEventService _eventService;

    public LecturerEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetLecturerEvents([FromQuery] GetLecturerEventsRequest request)
    {
        var result = await _eventService.GetLecturerEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetLecturerEventDetail(Guid eventId)
    {
        var result = await _eventService.GetLecturerEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
