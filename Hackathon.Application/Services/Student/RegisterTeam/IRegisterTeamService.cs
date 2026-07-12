namespace Hackathon.Application.Services.Student.RegisterTeam;

public interface IRegisterTeamService
{
    Task<GetRegisterTeamsResponse> GetRegisterTeams(GetRegisterTeamsRequest request);
    Task<RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task<GetRegisterTeamsResponse> GetRegisterTeamsByTeam(GetRegisterTeamsByTeamRequest request);
    Task<GetRegisterTeamsResponse> GetTeamRegisterTeams(Guid teamId, int pageIndex, int pageSize);
    Task<GetUserEventsResponse> GetUserEvents(GetUserEventsRequest request);
}
