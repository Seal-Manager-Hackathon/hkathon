namespace Hackathon.Application.Services.Lecturer.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request);
    Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(Guid teamId, GetRegisterTeamsByTeamRequest request);
    Task<GetUserEventsResponse> GetUserEvents(Guid userId, GetUserEventsRequest request);
}