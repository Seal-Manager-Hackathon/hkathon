using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Judge;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Judge;

[Route("api/v1/judge")]
[ApiController]
public class JudgeController : ControllerBase
{
    private readonly IJudgeService _judgeService;

    public JudgeController(IJudgeService judgeService)
    {
        _judgeService = judgeService;
    }

    /// <summary>
    /// Lấy danh sách track được phân công trong 1 event (giống Mentor)
    /// </summary>
    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetMyTracks(Guid eventId)
    {
        var result = await _judgeService.GetMyTracks(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Submissions trong 1 track, có filter round + isGraded
    /// </summary>
    [HttpGet("tracks/{trackId:guid}/submissions")]
    public async Task<IActionResult> GetTrackSubmissions(
        Guid trackId,
        [FromQuery] Guid? roundId,
        [FromQuery] bool? isGraded,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetTrackSubmissions(trackId, roundId, isGraded, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách bài chưa chấm của judge
    /// </summary>
    [HttpGet("events/{eventId:guid}/submissions/pending")]
    public async Task<IActionResult> GetPendingSubmissions(
        Guid eventId,
        [FromQuery] Guid? trackId,
        [FromQuery] Guid? roundId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetPendingSubmissions(eventId, trackId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Tiêu chí chấm điểm cho 1 submission
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}/criteria")]
    public async Task<IActionResult> GetSubmissionCriteria(Guid submissionId)
    {
        var result = await _judgeService.GetSubmissionCriteria(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Xem lại điểm judge đã chấm cho 1 bài
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}/my-score")]
    public async Task<IActionResult> GetMySubmissionScore(Guid submissionId)
    {
        var result = await _judgeService.GetMySubmissionScore(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chấm điểm 1 bài nộp
    /// </summary>
    [HttpPost("submissions/{submissionId:guid}/score")]
    public async Task<IActionResult> SubmitScore(Guid submissionId, [FromBody] SubmitScoreRequest request)
    {
        var result = await _judgeService.SubmitScore(submissionId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Submitted Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sửa toàn bộ điểm
    /// </summary>
    [HttpPatch("scores/{scoreId:guid}")]
    public async Task<IActionResult> UpdateScore(Guid scoreId, [FromBody] SubmitScoreRequest request)
    {
        var result = await _judgeService.UpdateScore(scoreId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sửa 1 item điểm riêng lẻ
    /// </summary>
    [HttpPatch("scores/{scoreId:guid}/items/{scoreItemId:guid}")]
    public async Task<IActionResult> UpdateScoreItem(Guid scoreId, Guid scoreItemId, [FromBody] UpdateScoreItemRequest request)
    {
        var result = await _judgeService.UpdateScoreItem(scoreId, scoreItemId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Item Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Finalize điểm
    /// </summary>
    [HttpPost("scores/{scoreId:guid}/finalize")]
    public async Task<IActionResult> FinalizeScore(Guid scoreId)
    {
        var result = await _judgeService.FinalizeScore(scoreId);
        return Ok(ApiResponseFactory.Success(result, message: result, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách điểm judge đã chấm trong 1 event
    /// </summary>
    [HttpGet("events/{eventId:guid}/my-scores")]
    public async Task<IActionResult> GetMyScores(
        Guid eventId,
        [FromQuery] Guid? trackId,
        [FromQuery] bool? isGraded,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetMyScores(eventId, trackId, isGraded, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
