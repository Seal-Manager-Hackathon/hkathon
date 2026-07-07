using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.RegisterTeam;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminRegisterTeamController : ControllerBase
{
    private readonly IRegisterTeamService _registerTeamService;

    public AdminRegisterTeamController(IRegisterTeamService registerTeamService)
    {
        _registerTeamService = registerTeamService;
    }

    [HttpGet("events/{eventId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeams(Guid eventId, [FromQuery] GetRegisterTeamsRequest request)
    {
        request.EventId = eventId;
        var result = await _registerTeamService.GetRegisterTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> GetRegisterTeamDetail(Guid registerTeamId)
    {
        var result = await _registerTeamService.GetRegisterTeamDetail(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> UpdateRegisterTeam(Guid registerTeamId, [FromBody] UpdateRegisterTeamRequest request)
    {
        request.RegisterTeamId = registerTeamId;
        await _registerTeamService.UpdateRegisterTeam(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.RegisterTeamUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/approve")]
    public async Task<IActionResult> ApproveRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.ApproveRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/reject")]
    public async Task<IActionResult> RejectRegisterTeam(Guid registerTeamId, [FromBody] RejectRegisterTeamRequest body)
    {
        await _registerTeamService.RejectRegisterTeam(registerTeamId, body.RejectionReason);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}/events")]
    public async Task<IActionResult> GetUserEvents(Guid userId, [FromQuery] GetUserEventsRequest request)
    {
        request.UserId = userId;
        var result = await _registerTeamService.GetUserEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTeam(Guid teamId, [FromQuery] GetRegisterTeamsByTeamRequest request)
    {
        request.TeamId = teamId;
        var result = await _registerTeamService.GetRegisterTeamsByTeam(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/ban")]
    public async Task<IActionResult> BanRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.BanRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/unban")]
    public async Task<IActionResult> UnbanRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.UnbanRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
