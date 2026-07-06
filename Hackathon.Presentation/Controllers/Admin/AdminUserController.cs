using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminUserController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users/recent")]
    public async Task<IActionResult> GetRecentUsers()
    {
        var result = await _userService.GetRecentUsers();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentUsersFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/count")]
    public async Task<IActionResult> GetUserCount([FromQuery] GetUserCountRequest request)
    {
        var result = await _userService.GetUserCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest request)
    {
        var result = await _userService.GetAllUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserDetail(Guid userId)
    {
        var result = await _userService.GetUserDetail(new GetUserDetailRequest { UserId = userId });
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUser(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }
}
