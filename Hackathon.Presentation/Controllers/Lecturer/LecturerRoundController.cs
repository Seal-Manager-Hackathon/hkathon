using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using BaseRound = Hackathon.Application.Services.Base.Round;
using LecturerRound = Hackathon.Application.Services.Lecturer.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerRoundController : ControllerBase
{
    private readonly LecturerRound.IRoundService _lecturerRoundService;
    private readonly BaseRound.IRoundService _baseRoundService;

    public LecturerRoundController(
        LecturerRound.IRoundService lecturerRoundService,
        BaseRound.IRoundService baseRoundService)
    {
        _lecturerRoundService = lecturerRoundService;
        _baseRoundService = baseRoundService;
    }

    [HttpGet("events/{eventId:guid}/rounds")]
    public async Task<IActionResult> GetRounds(Guid eventId, [FromQuery] string? keyword, [FromQuery] int? roundNo)
    {
        var result = await _lecturerRoundService.GetRounds(eventId, keyword, roundNo);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("rounds/{roundId:guid}")]
    public async Task<IActionResult> GetRoundDetail(Guid roundId)
    {
        var result = await _baseRoundService.GetRoundDetail(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
