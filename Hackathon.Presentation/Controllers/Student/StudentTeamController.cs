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

    [HttpGet("teams/{teamId:guid}/events")]
    public async Task<IActionResult> GetTeamEvents(Guid teamId, [FromQuery] GetTeamEventsRequest request)
    {
        request.TeamId = teamId;
        var result = await _teamService.GetTeamEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
