using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Assign;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin/assign")]
[ApiController]
public class AdminAssignController : ControllerBase
{
    private readonly IAssignService _assignService;

    public AdminAssignController(IAssignService assignService)
    {
        _assignService = assignService;
    }

    [HttpGet("events/{eventId:guid}/users/assigned")]
    public async Task<IActionResult> GetAllAssignedUsers(Guid eventId, [FromQuery] GetAllAssignedUsersRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAllAssignedUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events/{eventId:guid}/assign/users")]
    public async Task<IActionResult> AssignUserToEvent(Guid eventId, [FromBody] AssignUserToEventRequest request)
    {
        request.EventId = eventId;
        await _assignService.AssignUserToEvent(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("event-assigns/{assignEventId:guid}/event-role")]
    public async Task<IActionResult> AssignEventRoleToLecturer(Guid assignEventId, [FromBody] AssignEventRoleToLecturerRequest request)
    {
        request.AssignEventId = assignEventId;
        await _assignService.AssignEventRoleToLecturer(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/staff/available")]
    public async Task<IActionResult> GetAvailableStaff(Guid eventId, [FromQuery] GetAvailableUserRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAvailableStaff(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/assigned")]
    public async Task<IActionResult> GetAssignedUsers(Guid eventId, [FromQuery] GetAssignedUsersRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAssignedUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/lecturers/available")]
    public async Task<IActionResult> GetAvailableLecturer(Guid eventId, [FromQuery] GetAvailableUserRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAvailableLecturer(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("event-assigns/{assignEventId:guid}/tracks")]
    public async Task<IActionResult> AssignTrackToEvent(Guid assignEventId, [FromBody] AssignTrackToEventRequest request)
    {
        request.AssignEventId = assignEventId;
        await _assignService.AssignTrackToEvent(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpDelete("event-assigns/{assignEventId:guid}/tracks/{trackId:guid}")]
    public async Task<IActionResult> RemoveTrackFromEvent(Guid assignEventId, Guid trackId)
    {
        await _assignService.RemoveTrackFromEvent(assignEventId, trackId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }
}
