namespace Hackathon.Application.Services.Student.Submission;

public interface ISubmissionService
{
    Task<GetRegisterTeamSubmissionsResponse> GetRegisterTeamSubmissions(Guid registerTeamId, Guid? roundId);
    Task<SubmissionDetailResponse> GetSubmissionDetail(Guid submissionId);
    Task<CreateSubmissionResponse> CreateSubmission(CreateSubmissionRequest request);
}
