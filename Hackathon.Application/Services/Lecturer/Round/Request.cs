namespace Hackathon.Application.Services.Lecturer.Round;

public class GetRoundsRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public int? RoundNo { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}