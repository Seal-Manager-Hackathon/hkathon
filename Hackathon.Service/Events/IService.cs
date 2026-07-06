using Hackathon.Service.Models;

namespace Hackathon.Service.Events;

public interface IService
{
    // #{Public} #{Student}
    Task<BasePaginationResponse> GetEvents(Request.GetEventsRequest request);
    Task<Response.EventResponse> GetEvent(Guid eventId);
    Task<List<Response.EventParticipantResponse>> GetMostParticipants(int? limit, bool? isDisable);
    Task<BasePaginationResponse> GetJoinedEvents(Request.GetJoinedEventsRequest request);

    // #{Admin}
    Task<BasePaginationResponse> GetEventsForAdmin(Request.GetEventsForAdminRequest request);
    Task<Response.EventResponse> GetAdminEvent(Guid eventId);
    Task<Response.CreateEventResponse> CreateEvent(Request.CreateEventRequest request);
    Task<string> UpdateEvent(Guid eventId, Request.UpdateEventRequest request);
    Task<string> DeleteEvent(Guid eventId);
    Task<string> PublishEvent(Guid eventId);
    Task<string> UnpublishEvent(Guid eventId);
    Task<string> CloseEvent(Guid eventId);
    Task<string> RestoreEvent(Guid eventId);
    Task<Response.AssignStaffToEventResponse> AssignStaffToEvent(Guid eventId, Request.AssignStaffToEventRequest request);
    Task<BasePaginationResponse> GetAvailableStaff(Guid eventId, string? keyword, PaginationRequest paginationRequest);
    Task<BasePaginationResponse> GetEventAssignments(Guid eventId, PaginationRequest paginationRequest);
    Task<string> RemoveStaffAssignment(Guid assignEventId);
    Task<Response.CreateAwardResponse> CreateAward(Guid eventId, Request.CreateAwardRequest request);
    Task<string> UpdateAward(Guid id, Request.UpdateAwardRequest request);
    Task<string> DeleteAward(Guid awardId);
    Task<Response.SetupStatusResponse> GetSetupStatus(Guid eventId);

    // #{Admin} #{Staff}
    Task<Response.AssignEventToTrackResponse> AssignEventToTrack(Guid assignEventId, Request.AssignEventToTrackRequest request);
    Task<Guid> RemoveTrackAssignment(Guid assignTrackId);
    Task<string> UpdateLecturerRole(Guid id, Request.UpdateLecturerRoleRequest request);
    Task<string> RecalculateLeaderboard(Guid eventId);
    Task<string> LockLeaderboard(Guid eventId);
    Task<string> PublishLeaderboard(Guid eventId);

    // #{Public} #{Student} #{Admin} #{Staff}
    Task<List<Response.AwardResponse>> GetAwards(Guid eventId);
    Task<List<Response.LeaderboardResponse>> GetLeaderboard(Guid eventId);
    Task<Response.EventSummaryResponse> GetSummary(Guid eventId);
    Task<List<Response.TeamScoreResponse>> GetTeamScores(Guid eventId, Guid teamId);
}
