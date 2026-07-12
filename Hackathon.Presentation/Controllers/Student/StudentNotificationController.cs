using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using StudentNotification = Hackathon.Application.Services.Student.Notification;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentNotificationController : ControllerBase
{
    private readonly StudentNotification.INotificationService _notificationService;

    public StudentNotificationController(StudentNotification.INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Danh sach notification cua student (personal, team, system)
    /// </summary>
    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] StudentNotification.GetStudentNotificationsRequest request)
    {
        var result = await _notificationService.GetNotifications(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
