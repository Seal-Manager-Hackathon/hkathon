using System.Threading.RateLimiting;
using Hackathon.Application.Common.Models;
using Hackathon.Application.Exceptions;

namespace Hackathon.Presentation.Extentions;

public static class RateLimitExtensions
{
    public static void ConfigureRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                // Lấy thời gian còn lại thực tế do limiter tính cho partition hiện tại.
                // Các limiter built-in (Sliding/Fixed/TokenBucket) ghi metadata "RETRY_AFTER" (TimeSpan).
                TimeSpan retryAfter = TimeSpan.Zero;
                if (context.Lease is not null
                    && context.Lease.TryGetMetadata("RETRY_AFTER", out var retryMetadata)
                    && retryMetadata is TimeSpan retryTimespan)
                {
                    retryAfter = retryTimespan;
                }

                // Countdown cần ít nhất 1 giây để có ý nghĩa.
                var retryAfterSeconds = Math.Max(1, (int)Math.Ceiling(retryAfter.TotalSeconds));
                var retryAtUtc = DateTime.UtcNow.AddSeconds(retryAfterSeconds);

                var httpContext = context.HttpContext;
                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                httpContext.Response.Headers["Retry-After"] = retryAfterSeconds.ToString();

                // Khớp convention ErrorResponse của dự án (camelCase qua WriteAsJsonAsync),
                // đặt thông tin đếm ngược vào trường cấu trúc `error`.
                var response = ApiResponseFactory.Error(
                    title: "Too Many Requests",
                    status: StatusCodes.Status429TooManyRequests,
                    message: string.Format(ErrorMessage.Common.TooManyRequestsRetryAfter, retryAfterSeconds),
                    error: new
                    {
                        retryAfterSeconds = retryAfterSeconds,
                        retryAfter = $"{retryAfterSeconds}s",
                        retryAtUtc = retryAtUtc
                    },
                    traceId: httpContext.TraceIdentifier);

                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
            };

            // Global rate limiter applied to all endpoints
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    context.User.Identity?.Name
                    ?? context.Connection.RemoteIpAddress?.ToString()
                    ?? "anonymous",
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 500,
                        Window = TimeSpan.FromSeconds(5),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            options.AddPolicy("api", context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    context.User.Identity?.Name
                    ?? context.Connection.RemoteIpAddress?.ToString()
                    ?? "anonymous",
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0 // fail fast
                    }));

            // Strict login policy
            options.AddPolicy("login", context =>
                RateLimitPartition.GetFixedWindowLimiter(
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 20,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));

            // Heavy operation limit
            options.AddPolicy("heavy", context =>
                RateLimitPartition.GetTokenBucketLimiter(
                    context.User.Identity?.Name ?? "anonymous",
                    _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 10,
                        TokensPerPeriod = 5,
                        ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));
        });
    }
}
