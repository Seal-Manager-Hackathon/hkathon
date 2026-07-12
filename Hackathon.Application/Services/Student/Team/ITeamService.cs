namespace Hackathon.Application.Services.Student.Team;

public interface ITeamService
{
    Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request);
    Task<CreateTeamResponse> CreateTeam(CreateTeamRequest request);
    Task UpdateTeam(UpdateTeamRequest request);
    Task<GetMyTeamsResponse> GetMyTeams(GetMyTeamsRequest request);
    Task<GetTeamMembersResponse> GetTeamMembers(Guid teamId, int pageIndex, int pageSize);
    Task DisbandTeam(Guid teamId);
}
