using Hackathon.Application.Services.Admin.Award;

namespace Hackathon.Application.Services.Lecturer.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(Guid eventId, string? keyword);
}
