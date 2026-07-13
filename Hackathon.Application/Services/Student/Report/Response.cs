namespace Hackathon.Application.Services.Student.Report;

public class CreateReportResponse
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class GetReportsResponse
{
    public List<ReportItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class GetReportDetailResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public string? TypeReport { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
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
