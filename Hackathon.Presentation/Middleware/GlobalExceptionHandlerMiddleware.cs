using System.Text.Json;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Exceptions;

namespace Hackathon.Presentation.Middleware;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
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

            var appEx = ex switch
            {
                AppException alreadyAppEx => alreadyAppEx,
                JsonException jsonEx => jsonEx.Message.Contains("JSON") || jsonEx.Message.Contains("Path")
                    ? new BadRequestException(ErrorMessage.Common.InvalidJsonFormat)
                    : jsonEx.Message.Contains("enum") || jsonEx.Message.Contains("flag")
                        ? new BadRequestException(ErrorMessage.Common.InvalidEnumValue)
                        : new BadRequestException(ErrorMessage.Common.InvalidRequestData),
                _ => new ServerException(ErrorMessage.Common.UnexpectedError)
            };
            _logger.LogDebug("Mapped exception: {ExceptionType} -> {Message}", ex.GetType().Name, appEx.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = appEx.StatusCode;

            var response = ApiResponseFactory.Error(
                title: appEx.Title,
                status: appEx.StatusCode,
                message: appEx.Message,
                error: null,
                traceId: context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}