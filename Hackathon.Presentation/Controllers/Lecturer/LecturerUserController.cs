using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using AdminUser = Hackathon.Application.Services.Admin.User;
using LecturerUser = Hackathon.Application.Services.Lecturer.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerUserController : ControllerBase
{
    private readonly LecturerUser.IUserService _userService;

    public LecturerUserController(LecturerUser.IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Danh sách người dùng mới đăng ký.
    /// </summary>
    [HttpGet("users/recent")]
    public async Task<IActionResult> GetRecentUsers()
    {
        var result = await _userService.GetRecentUsers();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentUsersFetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Đếm số lượng người dùng.
    /// </summary>
    [HttpGet("users/count")]
    public async Task<IActionResult> GetUserCount([FromQuery] AdminUser.GetUserCountRequest request)
    {
        var result = await _userService.GetUserCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách tất cả người dùng.
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] AdminUser.GetAllUsersRequest request)
    {
        var result = await _userService.GetAllUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết người dùng.
    /// </summary>
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserDetail(Guid userId)
    {
        var result = await _userService.GetUserDetail(userId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}