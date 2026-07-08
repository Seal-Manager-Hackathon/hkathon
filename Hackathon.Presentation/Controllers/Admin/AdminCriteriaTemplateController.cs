using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.CriteriaTemplate;
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

    [HttpGet("criteria-templates/{templateId:guid}/criteria-items")]
    public async Task<IActionResult> GetCriteriaItemsByTemplate(Guid templateId, [FromQuery] GetCriteriaItemsByTemplateRequest request)
    {
        request.TemplateId = templateId;
        var result = await _criteriaTemplateService.GetCriteriaItemsByTemplate(request);
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

    [HttpPost("criteria-templates/{templateId:guid}/activate")]
    public async Task<IActionResult> ActivateCriteriaTemplate(Guid templateId)
    {
        await _criteriaTemplateService.ActivateCriteriaTemplate(templateId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-templates/{templateId:guid}/restore")]
    public async Task<IActionResult> RestoreCriteriaTemplate(Guid templateId)
    {
        await _criteriaTemplateService.RestoreCriteriaTemplate(templateId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("criteria-items/{itemId:guid}")]
    public async Task<IActionResult> UpdateCriteriaItem(Guid itemId, [FromBody] UpdateCriteriaItemRequest request)
    {
        request.ItemId = itemId;
        await _criteriaTemplateService.UpdateCriteriaItem(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-templates/{templateId:guid}/criteria-items")]
    public async Task<IActionResult> CreateCriteriaItem(Guid templateId, [FromBody] CreateCriteriaItemRequest request)
    {
        await _criteriaTemplateService.CreateCriteriaItem(templateId, request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-items/{itemId:guid}/delete")]
    public async Task<IActionResult> DeleteCriteriaItem(Guid itemId)
    {
        await _criteriaTemplateService.DeleteCriteriaItem(itemId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Deleted, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("criteria-items/{itemId:guid}/restore")]
    public async Task<IActionResult> RestoreCriteriaItem(Guid itemId)
    {
        await _criteriaTemplateService.RestoreCriteriaItem(itemId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.OperationSuccessful, traceId: HttpContext.TraceIdentifier));
    }
}
