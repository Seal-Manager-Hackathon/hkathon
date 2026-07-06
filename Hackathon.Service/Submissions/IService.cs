using Hackathon.Service.Models;

namespace Hackathon.Service.Submissions;

public interface IService
{
    // #{Public} #{Student}
    Task<Response.SubmissionDetailResponse> GetSubmissionDetail(Guid submissionId);
    Task<Response.SubmitRoundProjectResponse> SubmitRoundProject(Guid roundId, Guid registerTeamId, Request.SubmitRoundProjectRequest request);
    Task<BasePaginationResponse> GetSubmissions(Guid roundId, Guid registerTeamId, Request.GetSubmissionsRequest request);
}
