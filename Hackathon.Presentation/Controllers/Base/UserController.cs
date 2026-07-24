using Hackathon.Application.Common;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Base.User;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers.Base;

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
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
    {
        await _userProfileService.UpdateProfile(request);
        return Ok(ApiResponseFactory.Success<object?>(null, message: SuccessMessage.User.ProfileUpdated, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("avatar")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UpdateAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponseFactory.Error("Validation Error", 400, "File Is Required", traceId: HttpContext.TraceIdentifier));

        var avatarUrl = await _userProfileService.UpdateAvatar(file);
        return Ok(ApiResponseFactory.Success(new { avatarUrl }, message: SuccessMessage.User.AvatarUpdated, traceId: HttpContext.TraceIdentifier));
    }
}
