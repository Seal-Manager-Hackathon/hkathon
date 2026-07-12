namespace Hackathon.Application.Services.Student.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(GetAwardsRequest request);
    Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId);
}
