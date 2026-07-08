using System.ComponentModel.DataAnnotations;

namespace Hackathon.Application.Services.Admin.Report;

public class GetRecentReportsResponse
{
    public List<ReportItem> Reports { get; set; } = new();
}

public class GetReportsRequest
{
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
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
    public string UserEmail { get; set; } = null!;
    public string UserFirstName { get; set; } = null!;
    public string UserLastName { get; set; } = null!;
    public Guid? AssignEventId { get; set; }
    public string? AssignEventUserName { get; set; }
    public Guid? SubmissionId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImgUrl { get; set; }
    public string? FileUrl { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; }
    public string? TypeReport { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}

public class UpdateReportStatusRequest
{
    [Required(ErrorMessage = "Status Is Required")]
    public string Status { get; set; } = null!;
    public string? Reason { get; set; }
}

public class ReportItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; } = null!;
    public string UserFirstName { get; set; } = null!;
    public string UserLastName { get; set; } = null!;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? TypeReport { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
