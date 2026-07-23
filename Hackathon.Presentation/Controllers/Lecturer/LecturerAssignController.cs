using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Assign;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerAssignController : ControllerBase
{
    private readonly IAssignService _assignService;

    public LecturerAssignController(IAssignService assignService)
    {
        _assignService = assignService;
    }

    /// <summary>
    /// Danh sách nhân sự được phân công trong sự kiện.
    /// </summary>
    [HttpGet("events/{eventId:guid}/assign")]
    public async Task<IActionResult> GetAssignedUsers(Guid eventId, [FromQuery] GetAssignedUsersRequest request)
    {
        var result = await _assignService.GetAssignedUsers(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Thông tin phân công của chính giảng viên trong event.
    /// </summary>
    [HttpGet("events/{eventId:guid}/my-assignment")]
    public async Task<IActionResult> GetLecturerAssignedInfo(Guid eventId)
    {
        var result = await _assignService.GetLecturerAssignedInfo(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách staff khả dụng để phân công.
    /// </summary>
    [HttpGet("events/{eventId:guid}/staff/available")]
    public async Task<IActionResult> GetAvailableStaff(Guid eventId, [FromQuery] GetAvailableUserRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAvailableStaff(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách giảng viên khả dụng để phân công.
    /// </summary>
    [HttpGet("events/{eventId:guid}/lecturers/available")]
    public async Task<IActionResult> GetAvailableLecturer(Guid eventId, [FromQuery] GetAvailableUserRequest request)
    {
        request.EventId = eventId;
        var result = await _assignService.GetAvailableLecturer(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
