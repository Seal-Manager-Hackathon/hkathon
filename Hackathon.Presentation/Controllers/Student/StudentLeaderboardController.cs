using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Leaderboard;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentLeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public StudentLeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    /// <summary>
    /// API 1: Chapter year leaderboard — only published
    /// </summary>
    [HttpGet("events/chapter/{year:int}/leaderboard")]
    public async Task<IActionResult> GetChapterLeaderboard(int year, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _leaderboardService.GetChapterLeaderboard(year, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// API 2: Event leaderboard — only IsDisable=false
    /// </summary>
    [HttpGet("events/{eventId:guid}/leaderboard")]
    public async Task<IActionResult> GetEventLeaderboard(Guid eventId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _leaderboardService.GetEventLeaderboard(eventId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// API 3: My team's rank in chapter year leaderboard
    /// </summary>
    [HttpGet("leaderboard/my-year-rank")]
    public async Task<IActionResult> GetMyYearRank([FromQuery] GetMyYearRankRequest request)
    {
        var result = await _leaderboardService.GetMyYearRank(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// API 4: My year detail with per-event breakdown and event rank
    /// </summary>
    [HttpGet("leaderboard/my-year-detail")]
    public async Task<IActionResult> GetMyYearDetail([FromQuery] GetMyYearDetailRequest request)
    {
        var result = await _leaderboardService.GetMyYearDetail(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// API 5: My team's rank in event leaderboard
    /// </summary>
    [HttpGet("events/{eventId:guid}/leaderboard/my-rank")]
    public async Task<IActionResult> GetMyEventRank(Guid eventId)
    {
        var result = await _leaderboardService.GetMyEventRank(new GetMyEventRankRequest { EventId = eventId });
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
