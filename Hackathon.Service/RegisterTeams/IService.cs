using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.RegisterTeams;

public interface IService
{
    // #{Student}
    Task<(Response.RegisterTeamActionResponse Data, string Message)> RegisterEvent(Request.RegisterEventRequest request);
    Task<BasePaginationResponse> GetMyRegisteredEvents(Request.GetMyRegisteredEventsRequest request, PaginationRequest paginationRequest);
    Task<Response.RegisterTeamDetailForStudentResponse> GetRegisterTeamDetailForStudent(Guid registerId);
    Task<Response.RegisterTeamRejectionReasonResponse> GetRejectionReason(Guid registerId);
    Task<Response.RegisterTeamAssignmentStatusResponse> GetRegisterTeamAssignmentStatus(Guid registerTeamId);

    // #{Staff} #{Admin}
    Task<BasePaginationResponse> GetRegisterTeamsByEvent(Guid eventId, string? keyword, RegisterTeamStatusEnum? status, bool? isDisable, PaginationRequest paginationRequest);
    Task<Response.RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId);
    Task<Response.RegisterTeamActionResponse> AcceptRegisterTeam(Guid registerTeamId);
    Task<Response.RegisterTeamActionResponse> RejectRegisterTeam(Guid registerTeamId, Request.RejectRegisterTeamRequest request);
    Task<Response.RegisterTeamActionResponse> BanRegisterTeam(Guid registerTeamId, Request.BanTeamRequest request);
    Task<Response.RegisterTeamActionResponse> UnbanRegisterTeam(Guid registerTeamId);

    // #{Staff} #{Lecturer} #{Admin}
    Task<(List<Response.RegisterTeamTrackResponse> Data, string Message)> GetTeamsByTrack(Guid eventId, Guid trackId, Request.GetTeamsByTrackRequest request);
    Task<(List<Response.RegisterTeamApprovedResponse> Data, string Message)> GetApprovedTeams(Guid eventId, Request.GetApprovedTeamsRequest request);
    Task<(List<Response.RegisterTeamByRoundResponse> Data, string Message)> GetTeamsByRound(Guid eventId, Request.GetTeamsByRoundRequest request);
    Task<Response.TeamRoundSubmissionResponse> GetTeamRoundSubmissions(Guid registerTeamId, Guid? roundId);
}
