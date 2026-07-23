using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Event;
using Hackathon.Application.Services.Lecturer.Submission;
using Microsoft.AspNetCore.Mvc;
using LecturerEvent = Hackathon.Application.Services.Lecturer.Event;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerEventController : ControllerBase
{
    private readonly LecturerEvent.IEventService _eventService;
    private readonly ISubmissionService _submissionService;

    public LecturerEventController(LecturerEvent.IEventService eventService, ISubmissionService submissionService)
    {
        _eventService = eventService;
        _submissionService = submissionService;
    }

    /// <summary>
    /// Sự kiện mới nhất.
    /// </summary>
    [HttpGet("events/recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        var result = await _eventService.GetRecentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentEventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Đếm số lượng sự kiện.
    /// </summary>
    [HttpGet("events/count")]
    public async Task<IActionResult> GetEventCount([FromQuery] GetEventCountRequest request)
    {
        var result = await _eventService.GetEventCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách sự kiện giảng viên phụ trách.
    /// </summary>
    [HttpGet("events")]
    public async Task<IActionResult> GetLecturerEvents([FromQuery] LecturerEvent.GetLecturerEventsRequest request)
    {
        var result = await _eventService.GetLecturerEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sự kiện được gán cho giảng viên.
    /// </summary>
    [HttpGet("events/my-lecturer")]
    public async Task<IActionResult> GetLecturerAssignEvents([FromQuery] LecturerEvent.GetLecturerEventsRequest request)
    {
        var result = await _eventService.GetLecturerAssignEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết sự kiện.
    /// </summary>
    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetLecturerEventDetail(Guid eventId)
    {
        var result = await _eventService.GetLecturerEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách bài nộp trong sự kiện.
    /// </summary>
    [HttpGet("events/{eventId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissions(Guid eventId, [FromQuery] GetLecturerSubmissionsRequest request)
    {
        request.EventId = eventId;
        var result = await _submissionService.GetSubmissions(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết bài nộp.
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetSubmissionDetail(Guid submissionId)
    {
        var result = await _submissionService.GetSubmissionDetail(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}