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

    [HttpGet("rounds/{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> GetCriteriaTemplateByRoundId(Guid roundId)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplateByRoundId(roundId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-templates/{criteriaTemplateId:guid}/items")]
    public async Task<IActionResult> GetCriteriaItemsByTemplateId(Guid criteriaTemplateId)
    {
        var result = await _criteriaTemplateService.GetCriteriaItemsByTemplateId(criteriaTemplateId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-templates/{templateId:guid}")]
    public async Task<IActionResult> GetCriteriaTemplateDetail(Guid templateId)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplateDetail(templateId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-items/{itemId:guid}")]
    public async Task<IActionResult> GetCriteriaItemDetail(Guid itemId)
    {
        var result = await _criteriaTemplateService.GetCriteriaItemDetail(itemId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
