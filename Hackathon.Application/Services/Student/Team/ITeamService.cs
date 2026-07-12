namespace Hackathon.Application.Services.Student.Team;

public interface ITeamService
{
    Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request);
}
