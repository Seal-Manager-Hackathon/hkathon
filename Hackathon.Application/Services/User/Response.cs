using Hackathon.Domain.Enums.User;

namespace Hackathon.Application.Services.User;

public class GetUserCountResponse
{
    public int Total { get; set; }
}

public class GetRecentUsersResponse
{
    public List<RecentUserItem> Users { get; set; } = new();
}

public class RecentUserItem
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class CreateUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = null!;
    public string? College { get; set; }
}

