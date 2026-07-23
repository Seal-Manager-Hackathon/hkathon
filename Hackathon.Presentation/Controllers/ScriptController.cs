using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Script;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers;

[Route("api/v1/script")]
[ApiController]
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
    public async Task<IActionResult> BulkCreateUsers([FromBody] BulkCreateUsersRequest request)
    {
        var result = await _scriptService.BulkCreateUsers(request);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Admin.UsersBulkCreated, status: 201, traceId: HttpContext.TraceIdentifier));
    }
}
