namespace Hackathon.Application.Services.Staff.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(Guid eventId, GetAwardsRequest request);
    Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId);
}