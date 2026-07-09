using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Assign;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffAssignController : ControllerBase
{
    private readonly IAssignService _assignService;

    public StaffAssignController(IAssignService assignService)
    {
        _assignService = assignService;
    }

    [HttpGet("events/{eventId:guid}/lecturers/available")]
    public async Task<IActionResult> GetAvailableLecturers(Guid eventId, [FromQuery] GetAvailableLecturersRequest request)
    {
        var result = await _assignService.GetAvailableLecturers(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events/{eventId:guid}/assign/users")]
    public async Task<IActionResult> AssignLecturerToEvent(Guid eventId, [FromBody] AssignLecturerToEventRequest request)
    {
        await _assignService.AssignLecturerToEvent(eventId, request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/assigned")]
    public async Task<IActionResult> GetAssignedUsers(Guid eventId, [FromQuery] GetAssignedUsersRequest request)
    {
        var result = await _assignService.GetAssignedUsers(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/assigned/{assignEventId:guid}")]
    public async Task<IActionResult> GetAssignEventDetail(Guid eventId, Guid assignEventId)
    {
        var result = await _assignService.GetAssignEventDetail(eventId, assignEventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}