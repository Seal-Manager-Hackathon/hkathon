using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffRoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public StaffRoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpGet("events/{eventId:guid}/rounds")]
    public async Task<IActionResult> GetRounds(Guid eventId, [FromQuery] GetRoundsRequest request)
    {
        var result = await _roundService.GetRounds(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RoundsFetched, traceId: HttpContext.TraceIdentifier));
    }
}
