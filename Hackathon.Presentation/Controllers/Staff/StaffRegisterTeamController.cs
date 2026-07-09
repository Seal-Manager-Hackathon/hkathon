using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.RegisterTeam;
using Microsoft.AspNetCore.Mvc;
using StaffService = Hackathon.Application.Services.Staff.RegisterTeam.IRegisterTeamService;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffRegisterTeamController : ControllerBase
{
    private readonly StaffService _registerTeamService;

    public StaffRegisterTeamController(StaffService registerTeamService)
    {
        _registerTeamService = registerTeamService;
    }

    [HttpGet("events/{eventId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeams(Guid eventId, [FromQuery] GetRegisterTeamsRequest request)
    {
        var result = await _registerTeamService.GetRegisterTeams(eventId, request);
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
        await _registerTeamService.UpdateRegisterTeam(registerTeamId, request);
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

    [HttpGet("teams/{teamId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTeam(Guid teamId, [FromQuery] GetRegisterTeamsByTeamRequest request)
    {
        var result = await _registerTeamService.GetRegisterTeamsByTeam(teamId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}/events")]
    public async Task<IActionResult> GetUserEvents(Guid userId, [FromQuery] GetUserEventsRequest request)
    {
        var result = await _registerTeamService.GetUserEvents(userId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/ban")]
    public async Task<IActionResult> BanRegisterTeam(Guid registerTeamId, [FromBody] BanRegisterTeamRequest body)
    {
        await _registerTeamService.BanRegisterTeam(registerTeamId, body.RejectionReason);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/unban")]
    public async Task<IActionResult> UnbanRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.UnbanRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/assign-next-round")]
    public async Task<IActionResult> AssignToNextRound(Guid registerTeamId)
    {
        var result = await _registerTeamService.AssignToNextRound(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Created, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/revert-previous-round")]
    public async Task<IActionResult> RevertToPreviousRound(Guid registerTeamId)
    {
        var result = await _registerTeamService.RevertToPreviousRound(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/assign-track-topic")]
    public async Task<IActionResult> AssignTrackTopic(Guid registerTeamId, [FromBody] AssignTrackTopicRequest request)
    {
        await _registerTeamService.AssignTrackTopic(registerTeamId, request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("register-teams/{registerTeamId:guid}/remove-track-topic")]
    public async Task<IActionResult> RemoveTrackTopic(Guid registerTeamId)
    {
        await _registerTeamService.RemoveTrackTopic(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
