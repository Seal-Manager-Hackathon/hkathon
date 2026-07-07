namespace Hackathon.Application.Services.Award;

public interface IAwardService
{
    Task<GetAwardsResponse> GetAwards(GetAwardsRequest request);
    Task<CreateAwardResponse> CreateAward(CreateAwardRequest request);
    Task UpdateAward(UpdateAwardRequest request);
    Task DeleteAward(Guid awardId);
}
