using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Team;
using Hackathon.Application.Services.Admin.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminUserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;

    public AdminUserController(IUserService userService, ITeamService teamService)
    {
        _userService = userService;
        _teamService = teamService;
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

    [HttpPatch("users/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromForm] UpdateUserRequest request)
    {
        request.UserId = userId;
        await _userService.UpdateUser(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.UserUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUser(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UserCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users/{userId:guid}/delete")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await _userService.DeleteUser(userId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users/{userId:guid}/restore")]
    public async Task<IActionResult> RestoreUser(Guid userId)
    {
        await _userService.RestoreUser(userId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users/{userId:guid}/ban")]
    public async Task<IActionResult> BanUser(Guid userId, [FromBody] BanUserRequest request)
    {
        request.UserId = userId;
        await _userService.BanUser(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.UserBanned, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("users/{userId:guid}/unban")]
    public async Task<IActionResult> UnbanUser(Guid userId)
    {
        await _userService.UnbanUser(userId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.UserUnbanned, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}/teams")]
    public async Task<IActionResult> GetUserTeams(Guid userId, [FromQuery] GetUserTeamsRequest request)
    {
        request.UserId = userId;
        var result = await _teamService.GetUserTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
