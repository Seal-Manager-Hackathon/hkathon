using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Admin.Invitation;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Admin;

[Route("api/v1/admin")]
[ApiController]
public class AdminInvitationController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public AdminInvitationController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpGet("teams/{teamId:guid}/invitations")]
    public async Task<IActionResult> GetTeamInvitations(Guid teamId, [FromQuery] string? status, [FromQuery] string? keyword, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _invitationService.GetInvitations(teamId, status, keyword, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
