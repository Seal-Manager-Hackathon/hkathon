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
    /// Danh sách submissions trong event (myscope = bài được phân công theo track), filter track + round + status
    /// </summary>
    [HttpGet("events/{eventId:guid}/myscope")]
    public async Task<IActionResult> GetMyScope(
        Guid eventId,
        [FromQuery] Guid? trackId,
        [FromQuery] Guid? roundId,
        [FromQuery] string? status,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetMyScope(eventId, trackId, roundId, status, pageIndex, pageSize);
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
    /// Chấm điểm 1 bài nộp
    /// </summary>
    [HttpPost("submissions/{submissionId:guid}/scores")]
    public async Task<IActionResult> SubmitScore(Guid submissionId, [FromBody] SubmitScoreRequest request)
    {
        var result = await _judgeService.SubmitScore(submissionId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Submitted Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sửa toàn bộ điểm, trả về paginated score items với flag isUpdated
    /// </summary>
    [HttpPatch("scores/{scoreId:guid}")]
    public async Task<IActionResult> UpdateScore(
        Guid scoreId,
        [FromBody] SubmitScoreRequest request,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.UpdateScore(scoreId, request, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: "Score Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sửa 1 item điểm riêng lẻ (ko cần scoreId trên route)
    /// </summary>
    [HttpPatch("score-items/{scoreItemId:guid}")]
    public async Task<IActionResult> UpdateScoreItem(Guid scoreItemId, [FromBody] UpdateScoreItemRequest request)
    {
        var result = await _judgeService.UpdateScoreItem(scoreItemId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Item Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách bài nộp kèm điểm judge đã chấm trong 1 event, filter theo round, track, isGraded
    /// </summary>
    [HttpGet("events/{eventId:guid}/scores/me")]
    public async Task<IActionResult> GetMyScores(
        Guid eventId,
        [FromQuery] Guid? roundId,
        [FromQuery] Guid? trackId,
        [FromQuery] bool? isGraded,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetMyScores(eventId, roundId, trackId, isGraded, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Score items (điểm từng tiêu chí) của 1 lượt chấm — giống Admin, auth Judge
    /// </summary>
    [HttpGet("scores/{scoreId:guid}/items")]
    public async Task<IActionResult> GetScoreItems(
        Guid scoreId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetScoreItems(scoreId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Submissions per round cho 1 register team — last submission + judge score status
    /// </summary>
    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetRegisterTeamSubmissions(Guid registerTeamId)
    {
        var result = await _judgeService.GetRegisterTeamSubmissions(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiết 1 score item — giống Admin, auth Judge
    /// </summary>
    [HttpGet("score-items/{scoreItemId:guid}")]
    public async Task<IActionResult> GetScoreItemDetail(Guid scoreItemId)
    {
        var result = await _judgeService.GetScoreItemDetail(scoreItemId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
