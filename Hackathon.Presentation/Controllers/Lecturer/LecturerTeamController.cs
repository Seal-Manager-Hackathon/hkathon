using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Team;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerTeamController : ControllerBase
{
    private readonly ITeamService _teamService;

    public LecturerTeamController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet("teams/{teamId:guid}")]
    public async Task<IActionResult> GetTeamDetail(Guid teamId)
    {
        var result = await _teamService.GetTeamDetail(teamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.TeamDetailFetched, traceId: HttpContext.TraceIdentifier));
    }
}
