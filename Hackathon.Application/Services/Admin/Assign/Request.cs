namespace Hackathon.Application.Services.Admin.Assign;

public class GetAssignedUsersRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public string? Role { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllAssignedUsersRequest
{
    public string? Keyword { get; set; }
    public string? EventRole { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class AssignEventRoleToLecturerRequest
{
    public Guid AssignEventId { get; set; }
    public string EventRole { get; set; } = null!; // Judge or Mentor
}

public class AssignUserToEventRequest
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
}

public class GetAvailableUserRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
