using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Team;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentTeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public StudentTeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpPost("teams")]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request)
    {
        var result = await _teamService.CreateTeam(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Team.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/my-teams")]
    public async Task<IActionResult> GetMyTeams([FromQuery] GetMyTeamsRequest request)
    {
        var result = await _teamService.GetMyTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/count")]
    public async Task<IActionResult> GetTeamCount([FromQuery] GetTeamCountRequest request)
    {
        var result = await _teamService.GetTeamCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<IActionResult> GetTeamDetail(Guid teamId)
    {
        var result = await _teamService.GetTeamDetail(teamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/members")]
    public async Task<IActionResult> GetTeamMembers(Guid teamId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _teamService.GetTeamMembers(teamId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/events")]
    public async Task<IActionResult> GetTeamEvents(Guid teamId, [FromQuery] GetTeamEventsRequest request)
    {
        request.TeamId = teamId;
        var result = await _teamService.GetTeamEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("teams/{teamId:guid}")]
    public async Task<IActionResult> UpdateTeam(Guid teamId, [FromBody] UpdateTeamRequest request)
    {
        request.TeamId = teamId;
        await _teamService.UpdateTeam(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Team.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("teams/{teamId:guid}/disband")]
    public async Task<IActionResult> DisbandTeam(Guid teamId)
    {
        await _teamService.DisbandTeam(teamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Team.Deleted, traceId: HttpContext.TraceIdentifier));
    }
}
