namespace Hackathon.Application.Services.Base.RegisterTeam;

public class GetRegisterTeamRoundResponse
{
    public Guid RegisterTeamId { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }

    // Track / Topic
    public Guid? TrackId { get; set; }
    public string? TrackName { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }

    // Round hiện tại
    public Guid? CurrentRoundId { get; set; }
    public string? CurrentRoundName { get; set; }
    public int? CurrentRoundNo { get; set; }
}
