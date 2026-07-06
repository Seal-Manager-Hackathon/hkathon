namespace Hackathon.Application.Services.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request);
    Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task UpdateRegisterTeam(UpdateRegisterTeamRequest request);
    Task ApproveRegisterTeam(Guid registerTeamId);
    Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason);
}
