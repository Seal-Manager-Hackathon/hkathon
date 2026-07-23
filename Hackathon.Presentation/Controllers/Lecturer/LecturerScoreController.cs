using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Score;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;

    public LecturerScoreController(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    /// <summary>
    /// Chi tiết một lượt chấm điểm.
    /// </summary>
    [HttpGet("scores/{scoreId:guid}")]
    public async Task<IActionResult> GetScoreDetail(Guid scoreId)
    {
        var result = await _scoreService.GetScoreDetail(scoreId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách các lượt chấm điểm của các giám khảo cho bài nộp.
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}/grader-scores")]
    public async Task<IActionResult> GetSubmissionGraderScores(Guid submissionId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _scoreService.GetSubmissionGraderScores(submissionId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách điểm tiêu chí cụ thể.
    /// </summary>
    [HttpGet("scores/{scoreId:guid}/items")]
    public async Task<IActionResult> GetScoreItems(Guid scoreId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _scoreService.GetScoreItems(scoreId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Điểm của đội đăng ký trong round.
    /// </summary>
    [HttpGet("rounds/{roundId:guid}/register-teams/{registerTeamId:guid}/scores")]
    public async Task<IActionResult> GetTeamRoundScore(Guid roundId, Guid registerTeamId)
    {
        var result = await _scoreService.GetTeamRoundScore(roundId, registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết điểm một tiêu chí.
    /// </summary>
    [HttpGet("score-items/{scoreItemId:guid}")]
    public async Task<IActionResult> GetScoreItemDetail(Guid scoreItemId)
    {
        var result = await _scoreService.GetScoreItemDetail(scoreItemId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
