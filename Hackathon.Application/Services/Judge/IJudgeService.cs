using Hackathon.Application.Services.Admin.Score;

namespace Hackathon.Application.Services.Judge;

public interface IJudgeService
{
    Task<List<JudgeTrackItem>> GetMyTracks(Guid eventId);
    Task<GetTrackSubmissionsResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, bool? isGraded, int pageIndex, int pageSize);
    Task<GetTrackSubmissionsResponse> GetMyScope(Guid eventId, Guid? trackId, Guid? roundId, string? status, int pageIndex, int pageSize);
    Task<SubmissionCriteriaResponse> GetSubmissionCriteria(Guid submissionId);
    Task<JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, SubmitScoreRequest request);
    Task<UpdateScoreResponse> UpdateScore(Guid scoreId, SubmitScoreRequest request, int pageIndex = 1, int pageSize = 10);
    Task<UpdatedScoreItemResponse> UpdateScoreItem(Guid scoreItemId, UpdateScoreItemRequest request);
    Task<GetMyScoresResponse> GetMyScores(Guid eventId, Guid? roundId, Guid? trackId, bool? isGraded, int pageIndex, int pageSize);
    Task<GetScoreItemsResponse> GetScoreItems(Guid scoreId, int pageIndex, int pageSize);
    Task<ScoreItemDetail> GetScoreItemDetail(Guid scoreItemId);
    Task<GetRegisterTeamSubmissionsResponse> GetRegisterTeamSubmissions(Guid registerTeamId);
}
