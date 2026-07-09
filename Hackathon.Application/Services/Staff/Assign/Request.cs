namespace Hackathon.Application.Services.Staff.Assign;

public class GetAvailableLecturersRequest
{
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class AssignLecturerToEventRequest
{
    public Guid UserId { get; set; }
    public string EventRole { get; set; } = null!;
}

public class GetAssignedUsersRequest
{
    public string? Keyword { get; set; }
    public string? EventRole { get; set; }
    public string? Role { get; set; }
    public Guid? TrackId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class AssignTrackToEventRequest
{
    public Guid TrackId { get; set; }
}
