namespace Hackathon.Application.Services.Staff.Topic;

public class GetTopicsRequest
{
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
