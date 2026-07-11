using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerNotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public LecturerNotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var result = await _notificationService.GetNotifications(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/count")]
    public async Task<IActionResult> GetNotificationCount()
    {
        var result = await _notificationService.GetNotificationCount();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/recent")]
    public async Task<IActionResult> GetRecentNotifications()
    {
        var result = await _notificationService.GetRecentNotifications();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/unread-count")]
    public async Task<IActionResult> GetMyUnreadCount()
    {
        var result = await _notificationService.GetMyUnreadCount();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/{notificationId:guid}")]
    public async Task<IActionResult> GetNotificationDetail(Guid notificationId)
    {
        var result = await _notificationService.GetNotificationDetail(notificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/{notificationId:guid}/read")]
    public async Task<IActionResult> ReadNotification(Guid notificationId)
    {
        await _notificationService.ReadNotification(notificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/read-all")]
    public async Task<IActionResult> ReadAllNotifications()
    {
        await _notificationService.ReadAllNotifications();
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}