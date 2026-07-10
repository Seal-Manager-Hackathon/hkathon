using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.CriteriaTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerCriteriaTemplateController : ControllerBase
{
    private readonly ICriteriaTemplateService _criteriaTemplateService;

    public LecturerCriteriaTemplateController(ICriteriaTemplateService criteriaTemplateService)
    {
        _criteriaTemplateService = criteriaTemplateService;
    }

    [HttpGet("rounds/{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> GetCriteriaTemplatesByRound(Guid roundId, [FromQuery] string? keyword)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplatesByRound(roundId, keyword);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-templates/{templateId:guid}")]
    public async Task<IActionResult> GetCriteriaTemplateDetail(Guid templateId)
    {
        var result = await _criteriaTemplateService.GetCriteriaTemplateDetail(templateId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-templates/{templateId:guid}/criteria-items")]
    public async Task<IActionResult> GetCriteriaItemsByTemplate(Guid templateId)
    {
        var result = await _criteriaTemplateService.GetCriteriaItemsByTemplate(templateId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("criteria-items/{itemId:guid}")]
    public async Task<IActionResult> GetCriteriaItemDetail(Guid itemId)
    {
        var result = await _criteriaTemplateService.GetCriteriaItemDetail(itemId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
