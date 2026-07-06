using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Team;
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

    [HttpGet("teams/count")]
    public async Task<IActionResult> GetTeamCount([FromQuery] GetTeamCountRequest request)
    {
        var result = await _teamService.GetTeamCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamCountFetched, traceId: HttpContext.TraceIdentifier));
    }
}
