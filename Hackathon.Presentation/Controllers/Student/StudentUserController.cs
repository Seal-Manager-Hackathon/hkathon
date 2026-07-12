using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Student.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Student;

[Route("api/v1/student")]
[ApiController]
public class StudentUserController : ControllerBase
{
    private readonly IUserService _userService;

    public StudentUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetUserDetail(Guid userId)
    {
        var result = await _userService.GetUserDetail(userId);
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.Common.Fetched, traceId: HttpContext.TraceIdentifier));
    }
}
