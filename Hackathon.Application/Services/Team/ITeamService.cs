namespace Hackathon.Application.Services.Team;

public interface ITeamService
{
    Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request);
    Task<GetTeamsResponse> GetTeams(GetTeamsRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task UpdateTeam(UpdateTeamRequest request);
    Task DeleteTeam(Guid teamId);
    Task RestoreTeam(Guid teamId);
    Task<GetUserTeamsResponse> GetUserTeams(GetUserTeamsRequest request);
}
