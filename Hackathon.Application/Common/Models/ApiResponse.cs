namespace Hackathon.Application.Common.Models;
public abstract class ApiResponse
{
    public bool IsSuccess { get; set; }
    public bool IsFailed { get; set; }
    public int Status { get; set; }
    public object? Error { get; set; }
    public string? TraceId { get; set; }
    public DateTime TimestampUtc { get; set; }
}

public class BaseResponse : ApiResponse
{
    public object? Data { get; set; }
    public string? Message { get; set; }
}

public class ErrorResponse
{
    public string Title { get; set; } = null!;
    public int Status { get; set; }
    public string Message { get; set; } = null!;
    public string MessageCode { get; set; } = null!;
    public object? Errors { get; set; }
    public string? TraceId { get; set; }
    public DateTime TimestampUtc { get; set; }
}
