using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Event;
using Hackathon.Application.Services.RegisterTeam;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminEventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IRegisterTeamService _registerTeamService;

    public AdminEventController(IEventService eventService, IRegisterTeamService registerTeamService)
    {
        _eventService = eventService;
        _registerTeamService = registerTeamService;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request)
    {
        var result = await _eventService.GetEvents(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("events")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        await _eventService.CreateEvent(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.EventCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        var result = await _eventService.GetRecentEvents();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RecentEventsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}")]
    public async Task<IActionResult> GetEventDetail(Guid eventId)
    {
        var result = await _eventService.GetEventDetail(eventId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/count")]
    public async Task<IActionResult> GetEventCount([FromQuery] GetEventCountRequest request)
    {
        var result = await _eventService.GetEventCount(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.EventCountFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("events/{eventId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeams(Guid eventId, [FromQuery] GetRegisterTeamsRequest request)
    {
        request.EventId = eventId;
        var result = await _registerTeamService.GetRegisterTeams(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> GetRegisterTeamDetail(Guid registerTeamId)
    {
        var result = await _registerTeamService.GetRegisterTeamDetail(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamDetailFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPut("register-teams/{registerTeamId:guid}")]
    public async Task<IActionResult> UpdateRegisterTeam(Guid registerTeamId, [FromBody] UpdateRegisterTeamRequest request)
    {
        request.RegisterTeamId = registerTeamId;
        await _registerTeamService.UpdateRegisterTeam(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Admin.RegisterTeamUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("register-teams/{registerTeamId:guid}/approve")]
    public async Task<IActionResult> ApproveRegisterTeam(Guid registerTeamId)
    {
        await _registerTeamService.ApproveRegisterTeam(registerTeamId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("register-teams/{registerTeamId:guid}/reject")]
    public async Task<IActionResult> RejectRegisterTeam(Guid registerTeamId, [FromBody] RejectRegisterTeamRequest body)
    {
        await _registerTeamService.RejectRegisterTeam(registerTeamId, body.RejectionReason);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
