namespace Hackathon.Application.Services.Admin.Team;

public interface ITeamService
{
    Task<GetTeamCountResponse> GetTeamCount(GetTeamCountRequest request);
    Task<GetTeamsResponse> GetTeams(GetTeamsRequest request);
    Task<GetTeamDetailResponse> GetTeamDetail(Guid teamId);
    Task UpdateTeam(UpdateTeamRequest request);
    Task DeleteTeam(Guid teamId);
    Task RestoreTeam(Guid teamId);
    Task<GetUserTeamsResponse> GetUserTeams(GetUserTeamsRequest request);
    Task<GetTeamEventsResponse> GetTeamEvents(GetTeamEventsRequest request);
    Task<GetTeamMembersResponse> GetTeamMembers(Guid teamId, int pageIndex, int pageSize);
    Task LockTeam(Guid teamId);
    Task UnlockTeam(Guid teamId);
    Task ChangeLeader(Guid teamId, Guid newLeaderUserId);
}
