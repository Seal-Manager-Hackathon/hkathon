namespace Hackathon.Application.Services.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(GetRoundsRequest request);
    Task CreateRound(CreateRoundRequest request);
}
