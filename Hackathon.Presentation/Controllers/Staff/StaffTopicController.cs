using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Staff.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Staff;

[Route("api/v1/staff")]
[ApiController]
public class StaffTopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public StaffTopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("tracks/{trackId:guid}/topics")]
    public async Task<IActionResult> GetTopics(Guid trackId, [FromQuery] GetTopicsRequest request)
    {
        var result = await _topicService.GetTopics(trackId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("topics/{topicId:guid}")]
    public async Task<IActionResult> GetTopicDetail(Guid topicId)
    {
        var result = await _topicService.GetTopicDetail(topicId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}