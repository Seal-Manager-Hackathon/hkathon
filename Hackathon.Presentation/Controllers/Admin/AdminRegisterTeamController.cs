using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.RegisterTeam;
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

    [HttpPut("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> UpdateRegisterTeam(Guid registerTeamId, [FromBody] UpdateRegisterTeamRequest request)
    {
        request.RegisterTeamId = registerTeamId;
        await _registerTeamService.UpdateRegisterTeam(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.RegisterTeamUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("register-teams/{registerTeamId:guid}/approve")]
    public async Task<IActionResult> ApproveRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.ApproveRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("register-teams/{registerTeamId:guid}/reject")]
    public async Task<IActionResult> RejectRegisterTeam(Guid registerTeamId, [FromBody] RejectRegisterTeamRequest body)
    {
        await _registerTeamService.RejectRegisterTeam(registerTeamId, body.RejectionReason);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
