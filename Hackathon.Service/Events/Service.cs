using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Events;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly Rounds.IRoundEndScheduler _roundEndScheduler;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext, Rounds.IRoundEndScheduler roundEndScheduler)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
        _roundEndScheduler = roundEndScheduler;
    }

    private Guid GetCurrentUserId() => throw new NotImplementedException();
    private bool IsCurrentUserAdmin() => throw new NotImplementedException();
    private async Task EnsureStaffAssignedToEvent(Guid eventId) => throw new NotImplementedException();

    // #{Public} #{Student}
    public async Task<BasePaginationResponse> GetEvents(Request.GetEventsRequest request) => throw new NotImplementedException();
    public async Task<Response.EventResponse> GetEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<List<Response.EventParticipantResponse>> GetMostParticipants(int? limit, bool? isDisable) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetJoinedEvents(Request.GetJoinedEventsRequest request) => throw new NotImplementedException();

    // #{Admin}
    public async Task<BasePaginationResponse> GetEventsForAdmin(Request.GetEventsForAdminRequest request) => throw new NotImplementedException();
    public async Task<Response.EventResponse> GetAdminEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<Response.CreateEventResponse> CreateEvent(Request.CreateEventRequest request) => throw new NotImplementedException();
    public async Task<string> UpdateEvent(Guid eventId, Request.UpdateEventRequest request) => throw new NotImplementedException();
    public async Task<string> DeleteEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<string> PublishEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<string> UnpublishEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<string> CloseEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<string> RestoreEvent(Guid eventId) => throw new NotImplementedException();
    public async Task<Response.AssignStaffToEventResponse> AssignStaffToEvent(Guid eventId, Request.AssignStaffToEventRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetAvailableStaff(Guid eventId, string? keyword, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetEventAssignments(Guid eventId, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<string> RemoveStaffAssignment(Guid assignEventId) => throw new NotImplementedException();
    public async Task<Response.CreateAwardResponse> CreateAward(Guid eventId, Request.CreateAwardRequest request) => throw new NotImplementedException();
    public async Task<string> UpdateAward(Guid id, Request.UpdateAwardRequest request) => throw new NotImplementedException();
    public async Task<string> DeleteAward(Guid awardId) => throw new NotImplementedException();
    public async Task<Response.SetupStatusResponse> GetSetupStatus(Guid eventId) => throw new NotImplementedException();

    // #{Admin} #{Staff}
    public async Task<Response.AssignEventToTrackResponse> AssignEventToTrack(Guid assignEventId, Request.AssignEventToTrackRequest request) => throw new NotImplementedException();
    public async Task<Guid> RemoveTrackAssignment(Guid assignTrackId) => throw new NotImplementedException();
    public async Task<string> UpdateLecturerRole(Guid id, Request.UpdateLecturerRoleRequest request) => throw new NotImplementedException();
    public async Task<string> RecalculateLeaderboard(Guid eventId) => throw new NotImplementedException();
    public async Task<string> LockLeaderboard(Guid eventId) => throw new NotImplementedException();
    public async Task<string> PublishLeaderboard(Guid eventId) => throw new NotImplementedException();

    // #{Public} #{Student} #{Admin} #{Staff}
    public async Task<List<Response.AwardResponse>> GetAwards(Guid eventId) => throw new NotImplementedException();
    public async Task<List<Response.LeaderboardResponse>> GetLeaderboard(Guid eventId) => throw new NotImplementedException();
    public async Task<Response.EventSummaryResponse> GetSummary(Guid eventId) => throw new NotImplementedException();
    public async Task<List<Response.TeamScoreResponse>> GetTeamScores(Guid eventId, Guid teamId) => throw new NotImplementedException();
}
