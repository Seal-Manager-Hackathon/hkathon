using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Event;

public class CreateEventRequest
{
    [Required(ErrorMessage = "Name Is Required")]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required(ErrorMessage = "StartTime Is Required")]
    public DateTimeOffset StartTime { get; set; }

    [Required(ErrorMessage = "EndTime Is Required")]
    public DateTimeOffset EndTime { get; set; }

    public DateTimeOffset? RegisterLimitTime { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "LimitTeam Must Be Greater Than Or Equal To 0")]
    public int? LimitTeam { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MinMember Must Be Greater Than Or Equal To 0")]
    public int? MinMember { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "MaxMember Must Be Greater Than Or Equal To 0")]
    public int? MaxMember { get; set; }

    public string? Season { get; set; }
}

public class GetEventsRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetEventCountRequest
{
    /// <summary>
    /// Lọc theo trạng thái. Không truyền hoặc null = lấy tất cả.
    /// Enum: Draft, Published, Closed
    /// </summary>
    public string? Status { get; set; }
}
