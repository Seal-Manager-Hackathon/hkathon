using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Leaderboard;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminLeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public AdminLeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("rounds/{roundId:guid}/leaderboard")]
    public async Task<IActionResult> GetRoundLeaderboard(Guid roundId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _leaderboardService.GetRoundLeaderboard(roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/leaderboard")]
    public async Task<IActionResult> GetEventLeaderboard(Guid eventId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _leaderboardService.GetEventLeaderboard(eventId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/chapter/{year:int}/leaderboard")]
    public async Task<IActionResult> GetChapterLeaderboard(int year, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _leaderboardService.GetChapterLeaderboard(year, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
