using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.RegisterTeam;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/register-teams")]
[ApiController]
public class RegisterTeamController : ControllerBase
{
    private readonly IRegisterTeamRoundService _registerTeamRoundService;

    public RegisterTeamController(IRegisterTeamRoundService registerTeamRoundService)
    {
        _registerTeamRoundService = registerTeamRoundService;
    }

    [HttpGet("{registerTeamId:guid}/current-round")]
    public async Task<IActionResult> GetCurrentRound(Guid registerTeamId)
    {
        var result = await _registerTeamRoundService.GetCurrentRound(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
