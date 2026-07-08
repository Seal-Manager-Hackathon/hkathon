namespace Hackathon.Application.Services.Admin.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request);
    Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task UpdateRegisterTeam(UpdateRegisterTeamRequest request);
    Task ApproveRegisterTeam(Guid registerTeamId);
    Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason);
    Task<GetUserEventsResponse> GetUserEvents(GetUserEventsRequest request);
    Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(GetRegisterTeamsByTeamRequest request);
    Task BanRegisterTeam(Guid registerTeamId, string rejectionReason);
    Task UnbanRegisterTeam(Guid registerTeamId);
    Task<AssignToNextRoundResponse> AssignToNextRound(Guid registerTeamId);
    Task<AssignToNextRoundResponse> RevertToPreviousRound(Guid registerTeamId);
    Task AssignTrackTopic(Guid registerTeamId, AssignTrackTopicRequest request);
    Task RemoveTrackTopic(Guid registerTeamId);
}
