namespace Hackathon.Application.Services.Staff.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamsResponse> GetRegisterTeams(Guid eventId, GetRegisterTeamsRequest request);
    Task<GetRegisterTeamsWithScoresResponse> GetRegisterTeamsWithScores(Guid eventId, GetRegisterTeamsRequest request);
    Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task UpdateRegisterTeam(Guid registerTeamId, UpdateRegisterTeamRequest request);
    Task ApproveRegisterTeam(Guid registerTeamId);
    Task RejectRegisterTeam(Guid registerTeamId, string? rejectionReason);
    Task<GetUserEventsResponse> GetUserEvents(Guid userId, GetUserEventsRequest request);
    Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(Guid teamId, GetRegisterTeamsByTeamRequest request);
    Task BanRegisterTeam(Guid registerTeamId, string rejectionReason);
    Task UnbanRegisterTeam(Guid registerTeamId);
    Task<AssignToNextRoundResponse> AssignToNextRound(Guid registerTeamId);
    Task<AssignToNextRoundResponse> RevertToPreviousRound(Guid registerTeamId);
    Task AssignTrackTopic(Guid registerTeamId, AssignTrackTopicRequest request);
    Task RemoveTrackTopic(Guid registerTeamId);
}
