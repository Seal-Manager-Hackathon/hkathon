namespace Hackathon.Application.Services.Report;

public class GetRecentReportsResponse
{
    public List<ReportItem> Reports { get; set; } = new();
}

public class ReportItem
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? TypeReport { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
