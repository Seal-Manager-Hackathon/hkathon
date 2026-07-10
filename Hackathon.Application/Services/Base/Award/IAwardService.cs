using Hackathon.Application.Services.Admin.Award;

namespace Hackathon.Application.Services.Base.Award;

public interface IAwardService
{
    Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId);
}
