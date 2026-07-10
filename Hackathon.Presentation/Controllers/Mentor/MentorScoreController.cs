using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Mentor.Score;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Mentor;

[Route("api/v1/mentor")]
[ApiController]
public class MentorScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;

    public MentorScoreController(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    [HttpGet("rounds/{roundId:guid}/register-teams/{registerTeamId:guid}/scores")]
    public async Task<IActionResult> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        var result = await _scoreService.GetTeamRoundScore(roundId, registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
