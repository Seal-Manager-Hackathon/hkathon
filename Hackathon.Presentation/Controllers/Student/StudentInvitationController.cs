using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.Invitation;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentInvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public StudentInvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    /// <summary>
    /// Gui loi moi vao team (chi leader)
    /// </summary>
    [HttpPost("teams/{teamId:guid}/invitations")]
    public async Task<IActionResult> SendInvitation(Guid teamId, [FromBody] SendInvitationRequest request)
    {
        await _invitationService.SendInvitation(teamId, request.Email);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Team.InvitationSent, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sach loi moi da gui cua team (chi leader)
    /// </summary>
    [HttpGet("teams/{teamId:guid}/invitations")]
    public async Task<IActionResult> GetSentInvitations(Guid teamId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _invitationService.GetSentInvitations(teamId, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Danh sach loi moi user da nhan
    /// </summary>
    [HttpGet("invitations")]
    public async Task<IActionResult> GetReceivedInvitations([FromQuery] GetInvitationsRequest request)
    {
        var result = await _invitationService.GetReceivedInvitations(request.Keyword, request.Status, request.PageIndex, request.PageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiet loi moi (cho nguoi nhan hoac leader team gui)
    /// </summary>
    [HttpGet("invitations/{invitationId:guid}")]
    public async Task<IActionResult> GetInvitationDetail(Guid invitationId)
    {
        var result = await _invitationService.GetInvitationDetail(invitationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chi tiet team tu loi moi (public, ko can auth — ai co invitationId cung xem duoc)
    /// </summary>
    [HttpGet("invitations/{invitationId:guid}/team")]
    public async Task<IActionResult> GetInvitationTeamDetail(Guid invitationId)
    {
        var result = await _invitationService.GetInvitationTeamDetail(invitationId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Chap nhan loi moi vao team
    /// </summary>
    [HttpPost("invitations/{invitationId:guid}/accept")]
    public async Task<IActionResult> AcceptInvitation(Guid invitationId)
    {
        await _invitationService.AcceptInvitation(invitationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Tu choi loi moi vao team
    /// </summary>
    [HttpPost("invitations/{invitationId:guid}/reject")]
    public async Task<IActionResult> RejectInvitation(Guid invitationId)
    {
        await _invitationService.RejectInvitation(invitationId);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.Common.Updated, traceId: HttpContext.TraceIdentifier));
    }
}
