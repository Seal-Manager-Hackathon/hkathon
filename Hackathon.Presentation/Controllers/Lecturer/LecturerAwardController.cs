using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Award;
using LecturerAward = Hackathon.Application.Services.Lecturer.Award;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerAwardController : ControllerBase
{
    private readonly LecturerAward.IAwardService _lecturerAwardService;
    private readonly IAwardService _baseAwardService;

    public LecturerAwardController(
        LecturerAward.IAwardService lecturerAwardService,
        IAwardService baseAwardService)
    {
        _lecturerAwardService = lecturerAwardService;
        _baseAwardService = baseAwardService;
    }

    [HttpGet("events/{eventId:guid}/awards")]
    public async Task<IActionResult> GetAwards(Guid eventId, [FromQuery] string? keyword)
    {
        var result = await _lecturerAwardService.GetAwards(eventId, keyword);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("awards/{awardId:guid}")]
    public async Task<IActionResult> GetAwardDetail(Guid awardId)
    {
        var result = await _baseAwardService.GetAwardDetail(awardId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
