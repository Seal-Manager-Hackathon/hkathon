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
    /// Đếm số notification của student, có filter theo status
    /// </summary>
    [HttpGet("notifications/count")]
    public async Task<IActionResult> GetNotificationCount([FromQuery] string? status)
    {
        var result = await _notificationService.GetNotificationCount(status);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Đếm số notification chưa đọc
    /// </summary>
    [HttpGet("notifications/unread-count")]
    public async Task<IActionResult> GetMyUnreadCount()
    {
        var result = await _notificationService.GetMyUnreadCount();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết 1 notification (personal, team, system)
    /// </summary>
    [HttpGet("notifications/{notificationId:guid}")]
    public async Task<IActionResult> GetNotificationDetail(Guid notificationId)
    {
        var result = await _notificationService.GetNotificationDetail(notificationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Đọc 1 notification — đổi status thành Read
    /// </summary>
    [HttpPost("notifications/{notificationId:guid}/read")]
    public async Task<IActionResult> ReadNotification(Guid notificationId)
    {
        await _notificationService.ReadNotification(notificationId);
        return Ok(ApiResponseFactory.Success<object>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Đọc tất cả notification chưa đọc — đổi tất cả thành Read
    /// </summary>
    [HttpPost("notifications/read-all")]
    public async Task<IActionResult> ReadAllNotifications()
    {
        await _notificationService.ReadAllNotifications();
        return Ok(ApiResponseFactory.Success<object>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
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
