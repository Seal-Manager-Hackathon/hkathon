using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.Award;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/awards")]
[ApiController]
public class AwardController : ControllerBase
{
    private readonly IAwardService _awardService;

    public AwardController(IAwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet("{awardId:guid}")]
    public async Task<IActionResult> GetAwardDetail(Guid awardId)
    {
        var result = await _awardService.GetAwardDetail(awardId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
