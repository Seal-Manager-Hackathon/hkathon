namespace Hackathon.Application.Services.Admin.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(GetAwardsRequest request);
    Task<GetAwardDetailResponse> GetAwardDetail(Guid awardId);
    Task<CreateAwardResponse> CreateAward(CreateAwardRequest request);
    Task UpdateAward(UpdateAwardRequest request);
    Task DeleteAward(Guid awardId);
    Task RestoreAward(Guid awardId);
    Task SwapAwardLevel(Guid awardId, int targetLevel);
}
