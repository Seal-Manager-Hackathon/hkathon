using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers;

[Route("api/v1/notifications")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("{notificationId:guid}")]
    public async Task<IActionResult> GetNotificationDetail(Guid notificationId)
    {
        var result = await _notificationService.GetNotificationDetail(notificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
