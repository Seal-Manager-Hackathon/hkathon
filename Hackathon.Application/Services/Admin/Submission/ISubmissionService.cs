namespace Hackathon.Application.Services.Admin.Submission;

public interface ISubmissionService
{
    Task<GetSubmissionsResponse> GetSubmissions(GetSubmissionsRequest request);
    Task<GetSubmissionsResponse> GetSubmissionsByRound(Guid roundId, int pageIndex, int pageSize);
}
