using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Mentor.RegisterTeam;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Mentor;

[Route("api/v1/mentor")]
[ApiController]
public class MentorRegisterTeamController : ControllerBase
{
    private readonly IRegisterTeamService _registerTeamService;

    public MentorRegisterTeamController(IRegisterTeamService registerTeamService)
    {
        _registerTeamService = registerTeamService;
    }

    /// <summary>
    /// Danh sách đội đăng ký thuộc track (lọc từ khóa, phân trang).
    /// </summary>
    [HttpGet("tracks/{trackId:guid}/register-teams")]
    public async Task<IActionResult> GetRegisterTeamsByTrack(Guid trackId, [FromQuery] string? keyword, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _registerTeamService.GetRegisterTeamsByTrack(trackId, keyword, pageIndex, pageSize);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
