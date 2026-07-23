namespace Hackathon.Application.Services.Script;

public interface IScriptService
{
    Task<BulkCreateUsersResponse> BulkCreateUsers(BulkCreateUsersRequest request);
    Task<BulkCreateTeamResponse> BulkCreateTeam(BulkCreateTeamRequest request);
}
