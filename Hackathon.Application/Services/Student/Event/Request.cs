namespace Hackathon.Application.Services.Student.Event;

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
    public string? Status { get; set; }
}
