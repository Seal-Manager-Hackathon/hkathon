namespace Hackathon.Application.Services.Staff.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(Guid eventId, GetRoundsRequest request);
}
