using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Student.Report;

public class CreateReportRequest
{
    [Required(ErrorMessage = "Title Is Required")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    public string? TypeReport { get; set; }
}

public class GetReportsRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
