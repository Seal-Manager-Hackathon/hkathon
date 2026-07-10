using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Round;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/rounds")]
[ApiController]
public class RoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public RoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpGet("{roundId:guid}")]
    public async Task<IActionResult> GetRoundDetail(Guid roundId)
    {
        var result = await _roundService.GetRoundDetail(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
