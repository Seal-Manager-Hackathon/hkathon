using Hackathon.Application.Common.Helpers;
using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Application.Services.Admin.Submission;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Lecturer.Submission;

public interface ISubmissionService
{
    Task<GetLecturerSubmissionsResponse> GetSubmissions(GetLecturerSubmissionsRequest request);
    Task<GetLecturerSubmissionsResponse> GetSubmissionsByRegisterTeam(Guid registerTeamId, Guid? roundId, int pageIndex, int pageSize);
    Task<GetLecturerSubmissionsResponse> GetSubmissionsByTrack(Guid trackId, int pageIndex, int pageSize);
    Task<GetSubmissionDetailResponse> GetSubmissionDetail(Guid submissionId);
}
