using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.RegisterTeam;
using Hackathon.Application.Services.Lecturer.Submission;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerRegisterTeamController : ControllerBase
{
    private readonly IRegisterTeamService _registerTeamService;
    private readonly ISubmissionService _submissionService;

    public LecturerRegisterTeamController(IRegisterTeamService registerTeamService, ISubmissionService submissionService)
    {
        _registerTeamService = registerTeamService;
        _submissionService = submissionService;
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

    [HttpGet("teams/{teamId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTeam(Guid teamId, [FromQuery] GetRegisterTeamsByTeamRequest request)
    {
        var result = await _registerTeamService.GetRegisterTeamsByTeam(teamId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("users/{userId:guid}/events")]
    public async Task<IActionResult> GetUserEvents(Guid userId, [FromQuery] GetUserEventsRequest request)
    {
        var result = await _registerTeamService.GetUserEvents(userId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("tracks/{trackId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTrack(
        Guid trackId,
        [FromQuery] Guid? roundId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _registerTeamService.GetRegisterTeamsByTrack(trackId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.RegisterTeamsFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpGet("register-teams/{registerTeamId:guid}/submissions")]
    public async Task<IActionResult> GetSubmissionsByRegisterTeam(
        Guid registerTeamId,
        [FromQuery] Guid? roundId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _submissionService.GetSubmissionsByRegisterTeam(registerTeamId, roundId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Kiểm tra 1 register team còn đang thi đấu ko, kèm track/topic/round hiện tại
    /// </summary>
    [HttpGet("register-teams/{registerTeamId:guid}/competition-status")]
    public async Task<IActionResult> GetCompetitionStatus(Guid registerTeamId)
    {
        var result = await _registerTeamService.GetCompetitionStatus(registerTeamId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}