namespace Hackathon.Application.Services.Judge;

public interface IJudgeService
{
    Task<List<JudgeTrackItem>> GetMyTracks(Guid eventId);
    Task<GetTrackSubmissionsResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, bool? isGraded, int pageIndex, int pageSize);
    Task<GetTrackSubmissionsResponse> GetPendingSubmissions(Guid eventId, Guid? trackId, Guid? roundId, int pageIndex, int pageSize);
    Task<SubmissionCriteriaResponse> GetSubmissionCriteria(Guid submissionId);
    Task<JudgeSubmissionScoreResponse?> GetMySubmissionScore(Guid submissionId);
    Task<JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, SubmitScoreRequest request);
    Task<JudgeSubmissionScoreResponse> UpdateScore(Guid scoreId, SubmitScoreRequest request);
    Task<JudgeScoreItemResponse> UpdateScoreItem(Guid scoreId, Guid scoreItemId, UpdateScoreItemRequest request);
    Task<string> FinalizeScore(Guid scoreId);
    Task<GetMyScoresResponse> GetMyScores(Guid eventId, Guid? trackId, bool? isGraded, int pageIndex, int pageSize);
}
