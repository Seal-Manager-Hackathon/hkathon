using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Team;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminTeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public AdminTeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("teams")]
    public async Task<IActionResult> GetTeams([FromQuery] GetTeamsRequest request)
    {
        var result = await _teamService.GetTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<IActionResult> GetTeamDetail(Guid teamId)
    {
        var result = await _teamService.GetTeamDetail(teamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/events")]
    public async Task<IActionResult> GetTeamEvents(Guid teamId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _teamService.GetTeamEvents(new GetTeamEventsRequest { TeamId = teamId, PageIndex = pageIndex, PageSize = pageSize });
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/count")]
    public async Task<IActionResult> GetTeamCount([FromQuery] GetTeamCountRequest request)
    {
        var result = await _teamService.GetTeamCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/members")]
    public async Task<IActionResult> GetTeamMembers(Guid teamId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _teamService.GetTeamMembers(teamId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("teams/{teamId:guid}")]
    public async Task<IActionResult> UpdateTeam(Guid teamId, [FromBody] UpdateTeamRequest request)
    {
        request.TeamId = teamId;
        await _teamService.UpdateTeam(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.TeamUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("teams/{teamId:guid}/delete")]
    public async Task<IActionResult> DeleteTeam(Guid teamId)
    {
        await _teamService.DeleteTeam(teamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("teams/{teamId:guid}/restore")]
    public async Task<IActionResult> RestoreTeam(Guid teamId)
    {
        await _teamService.RestoreTeam(teamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("teams/{teamId:guid}/lock")]
    public async Task<IActionResult> LockTeam(Guid teamId)
    {
        await _teamService.LockTeam(teamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.TeamLocked, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("teams/{teamId:guid}/unlock")]
    public async Task<IActionResult> UnlockTeam(Guid teamId)
    {
        await _teamService.UnlockTeam(teamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.TeamUnlocked, traceId: HttpContext.TraceIdentifier));
    }
}
