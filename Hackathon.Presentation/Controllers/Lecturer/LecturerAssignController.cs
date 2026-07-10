using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Lecturer.Assign;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Lecturer;

[Route("api/v1/lecturer")]
[ApiController]
public class LecturerAssignController : ControllerBase
{
    private readonly IAssignService _assignService;

    public LecturerAssignController(IAssignService assignService)
    {
        _assignService = assignService;
    }

    [HttpGet("events/{eventId:guid}/assign")]
    public async Task<IActionResult> GetAssignedUsers(Guid eventId, [FromQuery] GetAssignedUsersRequest request)
    {
        var result = await _assignService.GetAssignedUsers(eventId, request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
