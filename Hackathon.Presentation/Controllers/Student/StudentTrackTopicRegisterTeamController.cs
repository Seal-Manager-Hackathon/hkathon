using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Track;
using Hackathon.Application.Services.Student.Topic;
using Hackathon.Application.Services.Student.RegisterTeam;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentTrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public StudentTrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet("events/{eventId:guid}/tracks")]
    public async Task<IActionResult> GetTracksByEvent(Guid eventId, [FromQuery] GetTracksByEventRequest request)
    {
        request.EventId = eventId;
        var result = await _trackService.GetTracksByEvent(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}")]
    public async Task<IActionResult> GetTrackDetail(Guid trackId)
    {
        var result = await _trackService.GetTrackDetail(trackId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}

[Route("api/v1/student")]
[ApiController]
public class StudentTopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public StudentTopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    [HttpGet("tracks/{trackId:guid}/topics")]
    public async Task<IActionResult> GetTopicsByTrack(Guid trackId, [FromQuery] GetTopicsByTrackRequest request)
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

[Route("api/v1/student")]
[ApiController]
public class StudentRegisterTeamController : ControllerBase
{
    private readonly IRegisterTeamService _registerTeamService;

    public StudentRegisterTeamController(IRegisterTeamService registerTeamService)
    {
        _registerTeamService = registerTeamService;
    }

    [HttpGet("events/{eventId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeams(Guid eventId, [FromQuery] GetRegisterTeamsRequest request)
    {
        request.EventId = eventId;
        var result = await _registerTeamService.GetRegisterTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> GetRegisterTeamDetail(Guid registerTeamId)
    {
        var result = await _registerTeamService.GetRegisterTeamDetail(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTeam(Guid teamId, [FromQuery] GetRegisterTeamsByTeamRequest request)
    {
        request.TeamId = teamId;
        var result = await _registerTeamService.GetRegisterTeamsByTeam(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("teams/{teamId:guid}/register-teams/all")]
    public async Task<IActionResult> GetTeamRegisterTeams(Guid teamId, [FromQuery] string? status, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _registerTeamService.GetTeamRegisterTeams(teamId, status, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}/events")]
    public async Task<IActionResult> GetUserEvents(Guid userId, [FromQuery] GetUserEventsRequest request)
    {
        request.UserId = userId;
        var result = await _registerTeamService.GetUserEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
