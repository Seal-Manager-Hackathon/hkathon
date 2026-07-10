using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Award;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffAwardController : ControllerBase
{
    private readonly IAwardService _awardService;

    public StaffAwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet("events/{eventId:guid}/awards")]
    public async Task<IActionResult> GetAwards(Guid eventId, [FromQuery] GetAwardsRequest request)
    {
        var result = await _awardService.GetAwards(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.AwardsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("awards/{awardId:guid}")]
    public async Task<IActionResult> GetAwardDetail(Guid awardId)
    {
        var result = await _awardService.GetAwardDetail(awardId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}