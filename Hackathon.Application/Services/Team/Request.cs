namespace Hackathon.Application.Services.Team;

public class GetUserTeamsRequest
{
    public Guid UserId { get; set; }
    public string? Keyword { get; set; }
    public string? Status { get; set; }
    public bool? IsDisable { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}



public class UpdateTeamRequest
{
    public Guid TeamId { get; set; }
    public string? Name { get; set; }
    public bool? CanEdit { get; set; }
    public bool? IsDisable { get; set; }
}

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

public class GetTeamCountRequest
{
    /// <summary>
    /// Lọc theo trạng thái disable. Không truyền = lấy tất cả.
    /// true = chỉ team bị disable, false = chỉ team không bị disable
    /// </summary>
    public bool? IsDisable { get; set; }
}
