using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.CriteriaTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffCriteriaTemplateController : ControllerBase
{
    private readonly ICriteriaTemplateService _criteriaTemplateService;

    public StaffCriteriaTemplateController(ICriteriaTemplateService criteriaTemplateService)
    {
        _criteriaTemplateService = criteriaTemplateService;
    }

    [HttpGet("events/{eventId:guid}/rounds/{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> GetCriteriaTemplateByRoundId(Guid eventId, Guid roundId)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplateByRoundId(eventId, roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/criteria-templates/{criteriaTemplateId:guid}/items")]
    public async Task<IActionResult> GetCriteriaItemsByTemplateId(Guid eventId, Guid criteriaTemplateId)
    {
        var result = await _criteriaTemplateService.GetCriteriaItemsByTemplateId(eventId, criteriaTemplateId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
