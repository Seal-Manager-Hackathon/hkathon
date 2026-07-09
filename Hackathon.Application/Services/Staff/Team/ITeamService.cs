namespace Hackathon.Application.Services.Staff.Team;

public interface ITeamService
{
    Task<GetTeamsResponse> GetTeams(GetTeamsRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<GetUserTeamsResponse> GetUserTeams(GetUserTeamsRequest request);
}