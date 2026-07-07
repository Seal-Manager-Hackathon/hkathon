namespace Hackathon.Application.Services.Admin.Score;

public interface IScoreService
{
    Task<GetSubmissionScoresResponse> GetSubmissionScores(Guid submissionId);
    Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize);
}
