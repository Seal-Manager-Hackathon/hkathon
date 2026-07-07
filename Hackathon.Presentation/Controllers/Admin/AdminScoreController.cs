using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Score;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminScoreController : ControllerBase
{
    private readonly IScoreService _scoreService;

    public AdminScoreController(IScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    [HttpGet("scores/{scoreId:guid}")]
    public async Task<IActionResult> GetScoreDetail(Guid scoreId)
    {
        var result = await _scoreService.GetScoreDetail(scoreId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("submissions/{submissionId:guid}/scores")]
    public async Task<IActionResult> GetSubmissionScores(Guid submissionId)
    {
        var result = await _scoreService.GetSubmissionScores(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("scores/{scoreId:guid}/items")]
    public async Task<IActionResult> GetScoreItems(Guid scoreId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _scoreService.GetScoreItems(scoreId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
