namespace Hackathon.Application.Services.Student.Assign;

public class GetAssignedUsersRequest
{
    public Guid EventId { get; set; }
    public string? Keyword { get; set; }
    public string? EventRole { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
