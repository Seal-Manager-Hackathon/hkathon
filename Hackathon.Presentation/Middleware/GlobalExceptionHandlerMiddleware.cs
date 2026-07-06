using Hackathon.Application.Common.Models;
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
            _logger.LogError(ex, "Unhandled Exception Path: {Path}", context.Request.Path);

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response Already Started: {Path}", context.Request.Path);
                throw;
            }

            var appEx = ex as AppException ?? new ServerException(ErrorMessage.Common.UnexpectedError);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = appEx.StatusCode;

            object? errorDetail = _environment.IsDevelopment()
                ? new { detail = ex.Message, stackTrace = ex.StackTrace }
                : null;

            var response = ApiResponseFactory.Error(
                title: appEx.Title,
                status: appEx.StatusCode,
                message: appEx.Message,
                error: errorDetail,
                traceId: context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}