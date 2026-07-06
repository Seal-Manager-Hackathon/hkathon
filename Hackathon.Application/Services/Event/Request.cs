namespace Hackathon.Application.Services.Event;

public class GetEventCountRequest
{
    /// <summary>
    /// Lọc theo trạng thái. Không truyền hoặc null = lấy tất cả.
    /// Enum: Draft, Published, Closed
    /// </summary>
    public string? Status { get; set; }
}
