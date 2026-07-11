using Hackathon.Application.Services.Admin.Team;

namespace Hackathon.Application.Services.Staff.Team;

public interface ITeamService
{
    Task<GetTeamsResponse> GetTeams(GetTeamsRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<GetUserTeamsResponse> GetUserTeams(GetUserTeamsRequest request);
    Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request);
    Task<Admin.Team.GetTeamCountResponse> GetTeamCount(Admin.Team.GetTeamCountRequest request);
    Task<Admin.Team.GetRecentTeamsResponse> GetRecentTeams();
}