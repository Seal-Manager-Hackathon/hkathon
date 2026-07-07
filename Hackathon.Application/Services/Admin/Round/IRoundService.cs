namespace Hackathon.Application.Services.Admin.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(GetRoundsRequest request);
    Task CreateRound(CreateRoundRequest request);
    Task UpdateRound(UpdateRoundRequest request);
    Task SwapRound(SwapRoundRequest request);
    Task<int?> GetMaxRoundNo(Guid eventId);
    Task DeleteRound(Guid roundId);
    Task RestoreRound(Guid roundId);
    Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId);
}
