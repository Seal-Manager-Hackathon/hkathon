using Hackathon.Service.Models;

namespace Hackathon.Service.Judges;

public interface IService
{
    // #{Judge}
    Task<List<Response.JudgeTrackResponse>> GetMyTracks();
    Task<BasePaginationResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, bool? isGraded, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetEventSubmissions(Guid eventId, Guid? trackId, Guid? roundId, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetPendingSubmissions(Guid eventId, Guid? trackId, Guid? roundId, bool? isGraded, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetCurrentEventPendingSubmissions(Guid? trackId, Guid? roundId, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> SearchSubmissions(Guid eventId, Guid? trackId, string? keyword, bool? isGraded, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetRegradeSubmissions(Guid? eventId, Guid? trackId, bool? isRegraded, PaginationRequest paginationRequest);
    Task<Response.SubmissionCriteriaResponse> GetSubmissionCriteria(Guid submissionId);
    Task<Response.JudgeSubmissionScoreResponse?> GetMySubmissionScore(Guid submissionId);
    Task<BasePaginationResponse> GetMyScores(Guid eventId, Guid? trackId, bool? isGraded, PaginationRequest paginationRequest);

    // #{Judge} — Scoring
    Task<Response.JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, Request.SubmitScoreRequest request);
    Task<Response.JudgeSubmissionScoreResponse> SubmitMockScore(Guid submissionId, Request.SubmitScoreRequest request);
    Task<Response.JudgeSubmissionScoreResponse> UpdateScore(Guid scoreId, Request.SubmitScoreRequest request);
    Task<Response.JudgeScoreItemResponse> UpdateScoreItem(Guid scoreId, Guid scoreItemId, Request.UpdateScoreItemRequest request);
    Task<string> FinalizeScore(Guid scoreId);
    Task<Response.JudgeSubmissionScoreResponse> SubmitRetakeScore(Guid scoreId, Request.SubmitScoreRequest request);

    // #{Judge}
    Task<(List<Response.JudgeTrackTeamResponse> Data, string Message)> GetJudgeTeamsByEvent(Guid eventId, Guid? roundId);
    Task<BasePaginationResponse> GetJudgeRoundTeams(Guid eventId, Guid roundId, Guid? trackId, string? status, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetJudgeTeamSubmissions(Guid registerTeamId, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetJudgeRoundAllSubmissions(Guid roundId, string? status, PaginationRequest paginationRequest);
}
