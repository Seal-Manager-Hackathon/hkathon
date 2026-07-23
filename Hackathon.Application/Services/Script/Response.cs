namespace Hackathon.Application.Services.Script;

public class BulkCreateUsersResponse
{
    public List<BulkCreateUserItem> Users { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}

public class BulkCreateUserItem
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string StudentId { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
