using Hackathon.Repository.Enum;
using Hackathon.Service.Models;

namespace Hackathon.Service.Tracks;

public interface IService
{
    // #{Public} #{Student}
    Task<BasePaginationResponse> GetTracks(Guid? eventId, string? keyword, bool? isDisable, PaginationRequest paginationRequest);
    Task<Response.TrackResponse> GetTrack(Guid trackId);
    Task<BasePaginationResponse> GetTopicsByTrack(Guid trackId, string? keyword, bool? isDisable, PaginationRequest paginationRequest);
    Task<Response.TrackTeamCountResponse> GetTrackTeamCount(Guid trackId);

    // #{Admin}
    Task<Response.TrackResponse> CreateTrack(Guid eventId, Request.CreateTrackRequest request);
    Task<Response.TrackResponse> UpdateTrack(Guid trackId, Request.UpdateTrackRequest request);
    Task<Response.TrackResponse> DeleteTrack(Guid trackId);
    Task<BasePaginationResponse> GetAdminTopicsByTrack(Guid trackId, string? keyword, bool? isDisable, PaginationRequest paginationRequest);

    // #{Admin} #{Staff}
    Task<Response.TrackResponse> UpdateTrackVisibility(Guid trackId, Request.UpdateTrackVisibilityRequest request);
    Task<BasePaginationResponse> GetApprovedTeamsByEvent(Guid eventId, string? keyword, RegisterTeamStatusEnum? status, bool? isDisable, PaginationRequest paginationRequest);
    Task<Response.TeamTrackAssignmentResponse> AssignTrackToTeam(Guid eventId, Guid teamId, Request.AssignTrackToTeamRequest request);
    Task<Response.TeamTopicAssignmentResponse> AssignTopicToTeam(Guid eventId, Guid teamId, Request.AssignTopicToTeamRequest request);

    // #{Staff}
    Task<BasePaginationResponse> GetTracksByEvent(Guid eventId, string? keyword, bool? isDisable, PaginationRequest paginationRequest);

    // #{Lecturer} #{Judge} #{Mentor}
    Task<Response.MyEventAssignmentResponse> GetMyEventAssignment(Guid eventId, EventRoleEnum? role);
}
