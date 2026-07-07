namespace Hackathon.Application.Services.Admin.Submission;

public interface ISubmissionService
{
    Task<GetSubmissionDetailResponse> GetSubmissionDetail(Guid submissionId);
    Task<GetSubmissionsResponse> GetSubmissions(GetSubmissionsRequest request);
    Task<GetSubmissionsResponse> GetSubmissionsByRound(Guid roundId, string? keyword, int pageIndex, int pageSize);
    Task<GetSubmissionsResponse> GetSubmissionsByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize);
    Task<GetSubmissionsResponse> GetSubmissionsByTrack(Guid trackId, int pageIndex, int pageSize);
}
