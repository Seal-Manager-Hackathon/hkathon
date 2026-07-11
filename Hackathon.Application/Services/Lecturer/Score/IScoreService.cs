using Hackathon.Application.Services.Admin.Score;

namespace Hackathon.Application.Services.Lecturer.Score;

public interface IScoreService
{
    Task<GetScoreDetailResponse> GetScoreDetail(Guid scoreId);
    Task<GetSubmissionGraderScoresResponse> GetSubmissionGraderScores(Guid submissionId, int pageIndex = 1, int pageSize = 10);
    Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex = 1, int pageSize = 10);
    Task<GetTeamRoundScoreResponse> GetTeamRoundScore(Guid roundId, Guid registerTeamId);
    Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId);
}
