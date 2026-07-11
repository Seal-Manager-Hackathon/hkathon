using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Mentor.MentorNotification;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Mentor;

[Route("api/v1/mentor")]
[ApiController]
public class MentorNotificationController : ControllerBase
{
    private readonly IMentorNotificationService _mentorNotificationService;

    public MentorNotificationController(IMentorNotificationService mentorNotificationService)
    {
        _mentorNotificationService = mentorNotificationService;
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetTracksByEvent(Guid eventId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mentorNotificationService.GetTracksByEvent(eventId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("tracks/{trackId:guid}/mentor-notifications")]
    public async Task<IActionResult> SendTrackNotification(Guid trackId, [FromBody] SendTrackNotificationRequest request)
    {
        var result = await _mentorNotificationService.SendTrackNotification(trackId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Created, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/mentor-notifications")]
    public async Task<IActionResult> GetTrackNotifications(Guid trackId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mentorNotificationService.GetTrackNotifications(trackId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("mentor-notifications/{mentorNotificationId:guid}")]
    public async Task<IActionResult> GetMentorNotificationDetail(Guid mentorNotificationId)
    {
        var result = await _mentorNotificationService.GetMentorNotificationDetail(mentorNotificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("mentor-notifications/{mentorNotificationId:guid}")]
    public async Task<IActionResult> UpdateMentorNotification(Guid mentorNotificationId, [FromBody] UpdateMentorNotificationRequest request)
    {
        await _mentorNotificationService.UpdateMentorNotification(mentorNotificationId, request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("mentor-notifications/{mentorNotificationId:guid}/delete")]
    public async Task<IActionResult> DeleteMentorNotification(Guid mentorNotificationId)
    {
        await _mentorNotificationService.DeleteMentorNotification(mentorNotificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("mentor-notifications/{mentorNotificationId:guid}/restore")]
    public async Task<IActionResult> RestoreMentorNotification(Guid mentorNotificationId)
    {
        await _mentorNotificationService.RestoreMentorNotification(mentorNotificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
