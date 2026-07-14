using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminNotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public AdminNotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("notifications/recent")]
    public async Task<IActionResult> GetRecentNotifications()
    {
        var result = await _notificationService.GetRecentNotifications();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.NotificationsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request)
    {
        var result = await _notificationService.GetNotifications(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.NotificationsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/{notificationId:guid}")]
    public async Task<IActionResult> GetNotificationDetail(Guid notificationId)
    {
        var result = await _notificationService.GetNotificationDetail(notificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.NotificationDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications")]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
    {
        await _notificationService.CreateNotification(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("notifications/{notificationId:guid}")]
    public async Task<IActionResult> UpdateNotification(Guid notificationId, [FromBody] UpdateNotificationRequest request)
    {
        request.NotificationId = notificationId;
        await _notificationService.UpdateNotification(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.NotificationUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/{notificationId:guid}/delete")]
    public async Task<IActionResult> DeleteNotification(Guid notificationId)
    {
        await _notificationService.DeleteNotification(new DeleteNotificationRequest { NotificationId = notificationId });
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/{notificationId:guid}/restore")]
    public async Task<IActionResult> RestoreNotification(Guid notificationId)
    {
        await _notificationService.RestoreNotification(notificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    // === My notifications (Personal + System) ===

    [HttpGet("notifications/my/{notificationId:guid}")]
    public async Task<IActionResult> GetMyNotificationDetail(Guid notificationId)
    {
        var result = await _notificationService.GetMyNotificationDetail(notificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.NotificationDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/my")]
    public async Task<IActionResult> GetMyNotifications([FromQuery] GetMyNotificationsRequest request)
    {
        var result = await _notificationService.GetMyNotifications(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("notifications/my/unread-count")]
    public async Task<IActionResult> GetMyUnreadCount()
    {
        var result = await _notificationService.GetMyUnreadCount();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/my/{notificationId:guid}/read")]
    public async Task<IActionResult> ReadNotification(Guid notificationId)
    {
        await _notificationService.ReadNotification(notificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("notifications/my/read-all")]
    public async Task<IActionResult> ReadAllNotifications()
    {
        await _notificationService.ReadAllNotifications();
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
