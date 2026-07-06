using Hackathon.Application.Exceptions;

namespace Hackathon.Presentation.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        IHostEnvironment environment,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UNHANDLED_EXCEPTION_PATH: {Path}",
                            context.Request.Path);

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("RESPONSE_ALREADY_STARTED: {Path}", context.Request.Path);
                throw;
            }

            context.Response.ContentType = "application/json";

            AppException appEx = ex switch
            {
                AppException alreadyAppEx => alreadyAppEx,
                _ => new ServerException("AN_UNEXPECTED_ERROR_OCCURRED")
            };

            context.Response.StatusCode = appEx.StatusCode;

            var response = new
            {
                title = appEx.Title,
                status = appEx.StatusCode,
                message = appEx.Message,
                messageCode = appEx.MessageCode,
                errors = _environment.IsDevelopment() ? new { detail = ex.Message } : null,
                traceId = context.TraceIdentifier,
                timestampUtc = DateTime.UtcNow
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
