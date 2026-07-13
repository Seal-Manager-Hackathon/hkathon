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

    /// <summary>
    /// Mentor notifications cho register team — lọc theo track của register team đó
    /// </summary>
    [HttpGet("register-teams/{registerTeamId:guid}/mentor-notifications")]
    public async Task<IActionResult> GetMentorNotificationsByRegisterTeam(
        Guid registerTeamId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _notificationService.GetMentorNotificationsByRegisterTeam(registerTeamId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết 1 mentor notification — event, track, mentor info
    /// </summary>
    [HttpGet("mentor-notifications/{mentorNotificationId:guid}")]
    public async Task<IActionResult> GetMentorNotificationDetail(Guid mentorNotificationId)
    {
        var result = await _notificationService.GetMentorNotificationDetail(mentorNotificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
