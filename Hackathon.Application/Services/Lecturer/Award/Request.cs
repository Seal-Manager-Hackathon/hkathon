namespace Hackathon.Application.Services.Lecturer.Award;

public class GetAwardsRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
