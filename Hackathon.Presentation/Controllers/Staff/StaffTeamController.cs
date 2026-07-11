using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using StaffTeam = Hackathon.Application.Services.Staff.Team;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffTeamController : ControllerBase
{
    private readonly StaffTeam.ITeamService _teamService;

    public StaffTeamController(StaffTeam.ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("teams/recent")]
    public async Task<IActionResult> GetRecentTeams()
    {
        var result = await _teamService.GetRecentTeams();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/count")]
    public async Task<IActionResult> GetTeamCount([FromQuery] Hackathon.Application.Services.Admin.Team.GetTeamCountRequest request)
    {
        var result = await _teamService.GetTeamCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams")]
    public async Task<IActionResult> GetTeams([FromQuery] StaffTeam.GetTeamsRequest request)
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
        var result = await _teamService.GetTeamEvents(new StaffTeam.GetTeamEventsRequest { TeamId = teamId, PageIndex = pageIndex, PageSize = pageSize });
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}