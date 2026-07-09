namespace Hackathon.Application.Services.Staff.Team;

public class GetTeamsRequest
{
    public string? Keyword { get; set; }
    public bool? CanEdit { get; set; }
    public bool? IsDisable { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserTeamsRequest
{
    public Guid UserId { get; set; }
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}