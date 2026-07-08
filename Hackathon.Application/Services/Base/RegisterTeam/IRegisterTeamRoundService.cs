namespace Hackathon.Application.Services.Base.RegisterTeam;

public interface IRegisterTeamRoundService
{
    Task<GetRegisterTeamRoundResponse> GetCurrentRound(Guid registerTeamId);
}
