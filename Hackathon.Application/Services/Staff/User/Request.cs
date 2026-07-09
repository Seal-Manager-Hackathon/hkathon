namespace Hackathon.Application.Services.Staff.User;

public class GetAllUsersRequest
{
    public string? Keyword { get; set; }
    public string? Role { get; set; }
    public bool? IsDisable { get; set; }
    public bool? IsVerified { get; set; }
    public bool? IsBanned { get; set; }
    public DateTimeOffset? FromDate { get; set; }
    public DateTimeOffset? ToDate { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetUserDetailRequest
{
    public Guid UserId { get; set; }
}