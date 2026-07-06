namespace Hackathon.Application.Common.Models;

public static class ApiResponseFactory
{
    public static ApiResponse<T> Success<T>(
        T? data, string? message = null, int status = 200, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = true,
            Status = status,
            Data = data,
            Message = message,
            TraceId = traceId
        };
    }

    public static ApiResponse<T> Fail<T>(
        int status, string message, object? error = null, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            IsSuccess = false,
            Status = status,
            Message = message,
            Error = error,
            TraceId = traceId
        };
    }

    public static PaginationResponse<T> Paginated<T>(
        List<T> items, int pageIndex, int pageSize, int totalCount,
        string? traceId = null, int status = 200)
    {
        return new PaginationResponse<T>
        {
            IsSuccess = true,
            Status = status,
            TraceId = traceId,
            Data = new PaginationValue<T>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount
            }
        };
    }

    public static ErrorResponse Error(
        string title, int status, string message,
        string messageCode, object? error = null, string? traceId = null)
    {
        return new ErrorResponse
        {
            IsSuccess = false,
            Title = title,
            Status = status,
            Message = message,
            MessageCode = messageCode,
            Error = error,
            TraceId = traceId
        };
    }
}