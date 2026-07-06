using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.Presentation.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(ApiResponseFactory.Success(result, status: 201, traceId: HttpContext.TraceIdentifier));
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var result = await _authService.VerifyEmailAsync(request);
        return Ok(ApiResponseFactory.Success(result, traceId: HttpContext.TraceIdentifier));
    }
}
