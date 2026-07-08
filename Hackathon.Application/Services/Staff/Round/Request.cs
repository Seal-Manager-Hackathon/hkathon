namespace Hackathon.Application.Services.Staff.Round;

public class GetRoundsRequest
{
    public string? Keyword { get; set; }
    public int? RoundNo { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
