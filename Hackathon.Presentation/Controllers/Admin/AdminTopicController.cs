using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminTopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public AdminTopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpPost("tracks/{trackId:guid}/topics")]
    public async Task<IActionResult> CreateTopic(Guid trackId, [FromBody] CreateTopicRequest request)
    {
        request.TrackId = trackId;
        var result = await _topicService.CreateTopic(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/topics")]
    public async Task<IActionResult> GetTopicsByTrack(Guid trackId, [FromQuery] GetTopicsByTrackRequest request)
    {
        request.TrackId = trackId;
        var result = await _topicService.GetTopicsByTrack(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPut("topics/{topicId:guid}")]
    public async Task<IActionResult> UpdateTopic(Guid topicId, [FromBody] UpdateTopicRequest request)
    {
        request.TopicId = topicId;
        await _topicService.UpdateTopic(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
