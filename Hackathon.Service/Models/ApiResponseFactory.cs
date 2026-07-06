namespace Hackathon.Service.Models;

public static class ApiResponseFactory
{
    public static BaseResponse Base(
        object? data, int status, string message, bool isSuccess = true,
        object? errorCustom = null, string? traceId = null)
    {
        return new BaseResponse
        {
            IsSuccess = isSuccess,
            IsFailed = !isSuccess,
            Status = status,
            Message = message,
            Data = isSuccess ? data : null,
            Error = !isSuccess ? errorCustom : null,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }

    public static BasePaginationResponse<T> BasePagination<T>(
        List<T> items, int pageIndex, int pageSize, int totalCount, string? traceId = null)
    {
        return new BasePaginationResponse<T>
        {
            IsSuccess = true,
            IsFailed = false,
            Status = 200,
            Error = null,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow,
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
        string messageCode, object? errors, string? traceId = null)
    {
        return new ErrorResponse
        {
            Title = title,
            Status = status,
            Message = message,
            MessageCode = messageCode,
            Errors = errors,
            TraceId = traceId,
            TimestampUtc = DateTime.UtcNow
        };
    }
}