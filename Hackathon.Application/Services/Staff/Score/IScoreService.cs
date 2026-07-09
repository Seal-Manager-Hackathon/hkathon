namespace Hackathon.Application.Services.Staff.Score;

public interface IScoreService
{
    Task<GetScoreDetailResponse> GetScoreDetail(Guid scoreId);
    Task<GetSubmissionGraderScoresResponse> GetSubmissionGraderScores(Guid submissionId, int pageIndex, int pageSize);
    Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize);
    Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId);
    Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId);
}
