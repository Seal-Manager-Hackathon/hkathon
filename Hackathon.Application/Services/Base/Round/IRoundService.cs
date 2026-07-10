using Hackathon.Application.Services.Admin.Round;

namespace Hackathon.Application.Services.Base.Round;

public interface IRoundService
{
    Task<GetRoundDetailResponse> GetRoundDetail(Guid roundId);
}
