using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Award;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminAwardController : ControllerBase
{
    private readonly IAwardService _awardService;

    public AdminAwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet("events/{eventId:guid}/awards")]
    public async Task<IActionResult> GetAwards(Guid eventId, [FromQuery] GetAwardsRequest request)
    {
        request.EventId = eventId;
        var result = await _awardService.GetAwards(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.AwardsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events/{eventId:guid}/awards")]
    public async Task<IActionResult> CreateAward(Guid eventId, [FromBody] CreateAwardRequest request)
    {
        request.EventId = eventId;
        var result = await _awardService.CreateAward(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.AwardCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("events/{eventId:guid}/awards/{awardId:guid}")]
    public async Task<IActionResult> UpdateAward(Guid eventId, Guid awardId, [FromBody] UpdateAwardRequest request)
    {
        request.EventId = eventId;
        request.AwardId = awardId;
        await _awardService.UpdateAward(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.AwardUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("events/{eventId:guid}/awards/{awardId:guid}/delete")]
    public async Task<IActionResult> DeleteAward(Guid eventId, Guid awardId)
    {
        await _awardService.DeleteAward(awardId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }
}
