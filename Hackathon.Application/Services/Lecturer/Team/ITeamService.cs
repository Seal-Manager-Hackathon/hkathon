namespace Hackathon.Application.Services.Lecturer.Team;

public interface ITeamService
{
    Task<GetTeamsResponse> GetTeams(GetTeamsRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request);
    Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request);
    Task<Admin.Team.GetRecentTeamsResponse> GetRecentTeams();
}