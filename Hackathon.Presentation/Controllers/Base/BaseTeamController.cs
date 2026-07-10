using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Team;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/teams")]
[ApiController]
public class BaseTeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public BaseTeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("{teamId:guid}")]
    public async Task<IActionResult> GetTeamDetail(Guid teamId)
    {
        var result = await _teamService.GetTeamDetail(teamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamDetailFetched, traceId: HttpContext.TraceIdentifier));
    }
}
