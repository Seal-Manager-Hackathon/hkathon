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
    /// Lấy danh sách track được phân công trong 1 event.
    /// </summary>
    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetMyTracks(Guid eventId)
    {
        var result = await _judgeService.GetMyTracks(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Submissions trong 1 track (lọc theo round, trạng thái đã chấm hay chưa, phân trang).
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
    /// Danh sách bài nộp được phân công chấm theo track trong 1 event (lọc track, round, status, phân trang).
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
    /// Tiêu chí chấm điểm của 1 bài nộp.
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}/criteria")]
    public async Task<IActionResult> GetSubmissionCriteria(Guid submissionId)
    {
        var result = await _judgeService.GetSubmissionCriteria(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Xem chi tiết bài nộp.
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}")]
    public async Task<IActionResult> GetSubmissionDetail(Guid submissionId)
    {
        var result = await _judgeService.GetSubmissionDetail(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chấm điểm cho bài nộp.
    /// </summary>
    [HttpPost("submissions/{submissionId:guid}/scores")]
    public async Task<IActionResult> SubmitScore(Guid submissionId, [FromBody] SubmitScoreRequest request)
    {
        var result = await _judgeService.SubmitScore(submissionId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Submitted Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Cập nhật toàn bộ điểm của Judge hiện tại cho bài nộp.
    /// </summary>
    [HttpPatch("submissions/{submissionId:guid}/scores")]
    public async Task<IActionResult> UpdateScoreBySubmission(
        Guid submissionId,
        [FromBody] SubmitScoreRequest request,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.UpdateScoreBySubmission(submissionId, request, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: "Score Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Sửa một tiêu chí điểm riêng lẻ.
    /// </summary>
    [HttpPatch("score-items/{scoreItemId:guid}")]
    public async Task<IActionResult> UpdateScoreItem(Guid scoreItemId, [FromBody] UpdateScoreItemRequest request)
    {
        var result = await _judgeService.UpdateScoreItem(scoreItemId, request);
        return Ok(ApiResponseFactory.Success(result, message: "Score Item Updated Successfully", traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sách bài nộp kèm điểm Judge đã chấm trong event (lọc round, track, đã chấm/chưa, phân trang).
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
    /// Xem chi tiết điểm từng tiêu chí của một lượt chấm.
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
    /// Lấy bài nộp cuối cùng kèm trạng thái chấm điểm của đội đăng ký qua các round.
    /// </summary>
    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetRegisterTeamSubmissions(Guid registerTeamId)
    {
        var result = await _judgeService.GetRegisterTeamSubmissions(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Xem bài nộp cuối cùng của các đội trong một round (lọc track, đã chấm/chưa).
    /// </summary>
    [HttpGet("rounds/{roundId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRound(
        Guid roundId,
        [FromQuery] Guid? trackId,
        [FromQuery] bool? isGraded,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _judgeService.GetSubmissionsByRound(roundId, trackId, isGraded, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Xem chi tiết điểm của một tiêu chí cụ thể.
    /// </summary>
    [HttpGet("score-items/{scoreItemId:guid}")]
    public async Task<IActionResult> GetScoreItemDetail(Guid scoreItemId)
    {
        var result = await _judgeService.GetScoreItemDetail(scoreItemId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Lấy điểm chi tiết do chính Judge chấm cho một bài nộp.
    /// </summary>
    [HttpGet("submissions/{submissionId:guid}/my-score")]
    public async Task<IActionResult> GetMyScoreBySubmission(Guid submissionId)
    {
        var result = await _judgeService.GetMyScoreBySubmission(submissionId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
