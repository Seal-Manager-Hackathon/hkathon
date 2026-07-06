namespace Hackathon.Application.Common.Models;
public abstract class ApiResponse
{
    public bool IsSuccess { get; set; }
    public int Status { get; set; }
    public string? TraceId { get; set; }
    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public object? Error { get; set; }
}

public class ErrorResponse : ApiResponse
{
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string MessageCode { get; set; } = null!;
    public object? Error { get; set; }
}