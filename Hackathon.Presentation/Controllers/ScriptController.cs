using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Script;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hackathon.Presentation.Extentions;
using Microsoft.AspNetCore.RateLimiting;

namespace Hackathon.Presentation.Controllers;

[Route("api/v1/script")]
[ApiController]
[Authorize(Policy = JwtExtensions.AdminPolicy)]
public class ScriptController : ControllerBase
{
    private readonly IScriptService _scriptService;

    public ScriptController(IScriptService scriptService)
    {
        _scriptService = scriptService;
    }

    /// <summary>
    /// Tạo nhanh số lượng tài khoản user theo role, prefix email.
    /// </summary>
    [HttpPost("bulk-create-users")]
    [EnableRateLimiting("heavy")]
    public async Task<IActionResult> BulkCreateUsers([FromBody] BulkCreateUsersRequest request)
    {
        var result = await _scriptService.BulkCreateUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UsersBulkCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Tạo team với leader và danh sách member theo email.
    /// </summary>
    [HttpPost("bulk-create-team")]
    [EnableRateLimiting("heavy")]
    public async Task<IActionResult> BulkCreateTeam([FromBody] BulkCreateTeamRequest request)
    {
        var result = await _scriptService.BulkCreateTeam(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Created, status: 201, traceId: HttpContext.TraceIdentifier));
    }
}
