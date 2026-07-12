namespace Hackathon.Application.Services.Lecturer.Submission;

public class GetLecturerSubmissionsRequest
{
    public Guid EventId { get; set; }
    public Guid? RoundId { get; set; }
    public Guid? TrackId { get; set; }
    public Guid? TopicId { get; set; }
    public Guid? RegisterTeamId { get; set; }
    public string? Keyword { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
