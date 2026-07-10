using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Team;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffTeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public StaffTeamController(ITeamService teamService)
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
}