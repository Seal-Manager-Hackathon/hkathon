using System.Collections;

namespace Hackathon.Service.Models;

public class PaginationValue
{
    public List<object> Items { get; set; } = new();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage => PageIndex * PageSize < TotalCount;   
    public bool HasPreviousPage => PageIndex > 1;
}

public class BasePaginationResponse : ApiResponse
{
    public PaginationValue Data { get; set; } = new();
}

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

    public static BasePaginationResponse BasePagination(
        IList? items, int pageIndex, int pageSize, int totalCount, string? traceId = null)
    {
        var newItems = new List<object>();
        if (items != null)
        {
            foreach (var item in items)
            {
                newItems.Add(item);
            }
        }
        return new BasePaginationResponse
        {
            IsSuccess = true,
            IsFailed = false,
            Error = null,
            TraceId = traceId,
            Data = new PaginationValue
            {
                Items = newItems,
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