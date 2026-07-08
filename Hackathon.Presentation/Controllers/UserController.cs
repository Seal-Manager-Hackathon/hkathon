using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers;

[Route("api/v1/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await _userProfileService.GetMyProfile();
        return Ok(ApiResponseFactory.Success(result, message: SuccessMessage.User.ProfileFetched, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPatch("me")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        await _userProfileService.UpdateProfile(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.User.ProfileUpdated, traceId: HttpContext.TraceIdentifier));
    }
}
