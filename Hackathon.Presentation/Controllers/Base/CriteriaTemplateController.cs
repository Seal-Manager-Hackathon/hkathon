using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.CriteriaTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

[Route("api/v1/rounds")]
[ApiController]
public class CriteriaTemplateController : ControllerBase
{
    private readonly ICriteriaTemplateService _criteriaTemplateService;

    public CriteriaTemplateController(ICriteriaTemplateService criteriaTemplateService)
    {
        _criteriaTemplateService = criteriaTemplateService;
    }

    [HttpGet("{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> GetCriteriaTemplatesByRound(Guid roundId)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplatesByRound(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
