using Hackathon.Application.Services.Admin.Round;

namespace Hackathon.Application.Services.Lecturer.Round;

public interface IRoundService
{
    Task<GetRoundsResponse> GetRounds(Guid eventId, string? keyword, int? roundNo);
}
