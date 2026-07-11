using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using AdminTopic = Hackathon.Application.Services.Admin.Topic;
using LecturerTopic = Hackathon.Application.Services.Lecturer.Topic;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerTopicController : ControllerBase
{
    private readonly LecturerTopic.ITopicService _topicService;

    public LecturerTopicController(LecturerTopic.ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("tracks/{trackId:guid}/topics")]
    public async Task<IActionResult> GetTopicsByTrack(Guid trackId, [FromQuery] AdminTopic.GetTopicsByTrackRequest request)
    {
        request.TrackId = trackId;
        var result = await _topicService.GetTopicsByTrack(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("topics/{topicId:guid}")]
    public async Task<IActionResult> GetTopicDetail(Guid topicId)
    {
        var result = await _topicService.GetTopicDetail(topicId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
