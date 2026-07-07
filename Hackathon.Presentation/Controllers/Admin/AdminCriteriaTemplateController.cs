using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.CriteriaTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminCriteriaTemplateController : ControllerBase
{
    private readonly ICriteriaTemplateService _criteriaTemplateService;

    public AdminCriteriaTemplateController(ICriteriaTemplateService criteriaTemplateService)
    {
        _criteriaTemplateService = criteriaTemplateService;
    }

    [HttpGet("rounds/{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> GetCriteriaTemplatesByRound(Guid roundId, [FromQuery] GetCriteriaTemplatesByRoundRequest request)
    {
        request.RoundId = roundId;
        var result = await _criteriaTemplateService.GetCriteriaTemplatesByRound(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("rounds/{roundId:guid}/criteria-templates")]
    public async Task<IActionResult> CreateCriteriaTemplate(Guid roundId, [FromBody] CreateCriteriaTemplateRequest request)
    {
        request.RoundId = roundId;
        await _criteriaTemplateService.CreateCriteriaTemplate(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("criteria-templates/{templateId:guid}")]
    public async Task<IActionResult> UpdateCriteriaTemplate(Guid templateId, [FromBody] UpdateCriteriaTemplateRequest request)
    {
        request.TemplateId = templateId;
        await _criteriaTemplateService.UpdateCriteriaTemplate(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-templates/{templateId:guid}/delete")]
    public async Task<IActionResult> DeleteCriteriaTemplate(Guid templateId)
    {
        await _criteriaTemplateService.DeleteCriteriaTemplate(templateId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-templates/{templateId:guid}/restore")]
    public async Task<IActionResult> RestoreCriteriaTemplate(Guid templateId)
    {
        await _criteriaTemplateService.RestoreCriteriaTemplate(templateId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }
}
