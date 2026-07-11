using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Award;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerAwardController : ControllerBase
{
    private readonly IAwardService _awardService;

    public LecturerAwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet("events/{eventId:guid}/awards")]
    public async Task<IActionResult> GetAwards(Guid eventId, [FromQuery] GetAwardsRequest request)
    {
        request.EventId = eventId;
        var result = await _awardService.GetAwards(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("awards/{awardId:guid}")]
    public async Task<IActionResult> GetAwardDetail(Guid awardId)
    {
        var result = await _awardService.GetAwardDetail(awardId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
