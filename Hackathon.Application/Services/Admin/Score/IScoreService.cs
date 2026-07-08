namespace Hackathon.Application.Services.Admin.Score;

public interface IScoreService
{
    Task<GetScoreDetailResponse> GetScoreDetail(Guid scoreId);
    Task<GetSubmissionScoresResponse> GetSubmissionScores(Guid submissionId);
    Task<GetSubmissionGraderScoresResponse> GetSubmissionGraderScores(Guid submissionId, int pageIndex, int pageSize);
    Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize);
    Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId);
    Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId);
}
