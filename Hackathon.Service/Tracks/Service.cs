using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Tracks;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }

    private Guid GetCurrentUserId() => throw new NotImplementedException();
    private bool IsCurrentUserAdmin() => throw new NotImplementedException();
    private async Task EnsureStaffAssignedToEvent(Guid eventId) => throw new NotImplementedException();

    // #{Public} #{Student}
    public async Task<BasePaginationResponse> GetTracks(Guid? eventId, string? keyword, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.TrackResponse> GetTrack(Guid trackId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetTopicsByTrack(Guid trackId, string? keyword, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.TrackTeamCountResponse> GetTrackTeamCount(Guid trackId) => throw new NotImplementedException();

    // #{Admin}
    public async Task<Response.TrackResponse> CreateTrack(Guid eventId, Request.CreateTrackRequest request) => throw new NotImplementedException();
    public async Task<Response.TrackResponse> UpdateTrack(Guid trackId, Request.UpdateTrackRequest request) => throw new NotImplementedException();
    public async Task<Response.TrackResponse> DeleteTrack(Guid trackId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetAdminTopicsByTrack(Guid trackId, string? keyword, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();

    // #{Admin} #{Staff}
    public async Task<Response.TrackResponse> UpdateTrackVisibility(Guid trackId, Request.UpdateTrackVisibilityRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetApprovedTeamsByEvent(Guid eventId, string? keyword, RegisterTeamStatusEnum? status, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.TeamTrackAssignmentResponse> AssignTrackToTeam(Guid eventId, Guid teamId, Request.AssignTrackToTeamRequest request) => throw new NotImplementedException();
    public async Task<Response.TeamTopicAssignmentResponse> AssignTopicToTeam(Guid eventId, Guid teamId, Request.AssignTopicToTeamRequest request) => throw new NotImplementedException();

    // #{Staff}
    public async Task<BasePaginationResponse> GetTracksByEvent(Guid eventId, string? keyword, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();

    // #{Lecturer} #{Judge} #{Mentor}
    public async Task<Response.MyEventAssignmentResponse> GetMyEventAssignment(Guid eventId, EventRoleEnum? role) => throw new NotImplementedException();
}
