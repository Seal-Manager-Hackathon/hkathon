namespace Hackathon.Application.Services.Student.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(GetRoundsRequest request);
    Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId);
}
