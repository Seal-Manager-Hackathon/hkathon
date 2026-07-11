namespace Hackathon.Application.Services.Lecturer.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(GetAwardsRequest request);
    Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId);
}
