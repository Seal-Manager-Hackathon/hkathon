namespace Hackathon.Application.Services.Lecturer.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(GetRoundsRequest request);
    Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId);
    Task<int?> GetMaxRoundNo(Guid eventId);
}