using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Team;
using Hackathon.Application.Services.Staff.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffUserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITeamService _teamService;

    public StaffUserController(IUserService userService, ITeamService teamService)
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

    [HttpGet("users/{userId:guid}/teams")]
    public async Task<IActionResult> GetUserTeams(Guid userId, [FromQuery] GetUserTeamsRequest request)
    {
        request.UserId = userId;
        var result = await _teamService.GetUserTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}