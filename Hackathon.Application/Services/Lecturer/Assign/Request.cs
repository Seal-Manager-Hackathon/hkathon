namespace Hackathon.Application.Services.Lecturer.Assign;

public class GetAssignedUsersRequest
{
    public string? Keyword { get; set; }
    public string? EventRole { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
