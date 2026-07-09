using Hackathon.Application.Services.Admin.Submission;

namespace Hackathon.Application.Services.Staff.Submission;

public interface ISubmissionService
{
    Task<GetSubmissionDetailResponse> GetDetail(Guid submissionId);
    Task<GetSubmissionsResponse> GetByEvent(Guid eventId, GetSubmissionsRequest request);
    Task<GetSubmissionsResponse> GetByRound(Guid roundId, string? keyword, int pageIndex, int pageSize);
    Task<GetSubmissionsResponse> GetByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize);
    Task<GetSubmissionsResponse> GetByTrack(Guid trackId, int pageIndex, int pageSize);
}
