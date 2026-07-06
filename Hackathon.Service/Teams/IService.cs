using Hackathon.Service.Models;

namespace Hackathon.Service.Teams;

public interface IService
{
    // #{Student}
    Task<Response.CreateTeamResponse> CreateTeam(Request.CreateTeamRequest request);
    Task<string> InviteMember(Guid teamId, Request.InviteMemberRequest request);
    Task<BasePaginationResponse> GetMyTeams(PaginationRequest paginationRequest);
    Task<Response.TeamDetailResponse> GetTeamDetail(Guid teamId);
    Task<string> UpdateTeam(Guid teamId, Request.UpdateTeamRequest request);
    Task<string> RemoveMembers(Guid teamId, Request.RemoveMembersRequest request);
    Task<string> TransferLeader(Guid teamId, Request.TransferLeaderRequest request);
    Task<BasePaginationResponse> GetTeamRegisteredEvents(Guid teamId, RegisterTeams.Request.GetTeamRegisteredEventsRequest request, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetMyRegistrationsByEvent(Request.GetMyRegistrationsByEventRequest request);
    Task<Response.CountResponse> GetApprovedEventsCount(Guid teamId);
    Task<Response.LatestRegisteredEventResponse?> GetLatestRegisteredEvent(Guid teamId);
    Task<List<Response.TeamMemberResponse>> GetTeamMembers(Guid teamId);
    Task<List<Response.TeamNotificationResponse>> GetTeamNotifications(Guid teamId);
    Task<BasePaginationResponse> GetMyTeamRegisterEvents(string? status, PaginationRequest paginationRequest);
    Task<string> LeaveTeam(Guid teamId);
    Task<Response.AppealResponse> AppealRound(Guid teamId, Guid roundId, Request.RoundAppealRequest request);
    Task<Response.AppealResponse> AppealSubmission(Guid teamId, Guid submissionId, Request.SubmissionAppealRequest request);

    // #{Admin}
    Task<BasePaginationResponse> GetAdminTeams(string? keyword, bool? isDisable, PaginationRequest paginationRequest);
    Task<string> DisableTeam(Guid teamId);
    Task<string> EnableTeam(Guid teamId);

    // #{Staff} #{Admin}
    Task<string> LockTeam(Guid teamId);
    Task<string> UnlockTeam(Guid teamId);
}
