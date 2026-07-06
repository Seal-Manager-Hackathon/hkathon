using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Notification;
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

    [HttpPut("notifications/{notificationId:guid}")]
    public async Task<IActionResult> UpdateNotification(Guid notificationId, [FromBody] UpdateNotificationRequest request)
    {
        request.NotificationId = notificationId;
        await _notificationService.UpdateNotification(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.NotificationUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("notifications/{notificationId:guid}/delete")]
    public async Task<IActionResult> DeleteNotification(Guid notificationId)
    {
        await _notificationService.DeleteNotification(new DeleteNotificationRequest { NotificationId = notificationId });
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("notifications/{notificationId:guid}/restore")]
    public async Task<IActionResult> RestoreNotification(Guid notificationId)
    {
        await _notificationService.RestoreNotification(notificationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
