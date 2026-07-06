using Hackathon.Service.Models;

namespace Hackathon.Service.Rounds;

public interface IService
{
    // #{Public} #{Student}
    Task<List<Response.RoundResponse>> GetRounds(Guid eventId);
    Task<Response.RoundDetailResponse> GetRound(Guid roundId);
    Task<List<Response.MyRoundResponse>> GetMyRounds(Guid? eventId, Guid teamId);
    Task<Response.MyRoundDetailResponse> GetMyRoundDetail(Guid registerTeamId);
    Task<BasePaginationResponse> GetMyRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query);
    Task<BasePaginationResponse> GetRoundRanking(Guid roundId, Request.GetSubmissionsQuery query);
    Task<Response.MyRoundScoreResponse> GetMyRoundScore(Guid roundId);
    Task<Response.TeamLatestSubmissionScoreResponse> GetTeamLatestSubmissionScore(Guid roundId, Guid teamId);
    Task<Response.CreateSubmissionResponse> CreateSubmission(Guid roundId, Request.CreateSubmissionRequest request);
    Task<BasePaginationResponse> GetRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query);

    // #{Staff}
    Task<BasePaginationResponse> GetStaffRoundSubmissions(Guid roundId, Request.GetStaffRoundSubmissionsQuery query);
    Task<Response.AssignJudgesToSubmissionResponse> AssignJudgesToSubmission(Guid submissionId, Request.AssignJudgesToSubmissionRequest request);

    // #{Admin} #{Staff}
    Task<Response.EndRoundResponse> EndRound(Guid roundId);

    // #{Admin}
    Task<string> EndRoundFinal(Guid roundId);
    Task UpdateRound(Guid roundId, Request.UpdateRoundRequest request);

    // #{Lecturer}
    Task<BasePaginationResponse> GetLecturerRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query);
}
