using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Teams;

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

    // #{Student}
    public async Task<Response.CreateTeamResponse> CreateTeam(Request.CreateTeamRequest request) => throw new NotImplementedException();
    public async Task<string> InviteMember(Guid teamId, Request.InviteMemberRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyTeams(PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.TeamDetailResponse> GetTeamDetail(Guid teamId) => throw new NotImplementedException();
    public async Task<string> UpdateTeam(Guid teamId, Request.UpdateTeamRequest request) => throw new NotImplementedException();
    public async Task<string> RemoveMembers(Guid teamId, Request.RemoveMembersRequest request) => throw new NotImplementedException();
    public async Task<string> TransferLeader(Guid teamId, Request.TransferLeaderRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetTeamRegisteredEvents(Guid teamId, RegisterTeams.Request.GetTeamRegisteredEventsRequest request, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyRegistrationsByEvent(Request.GetMyRegistrationsByEventRequest request) => throw new NotImplementedException();
    public async Task<Response.CountResponse> GetApprovedEventsCount(Guid teamId) => throw new NotImplementedException();
    public async Task<Response.LatestRegisteredEventResponse?> GetLatestRegisteredEvent(Guid teamId) => throw new NotImplementedException();
    public async Task<List<Response.TeamMemberResponse>> GetTeamMembers(Guid teamId) => throw new NotImplementedException();
    public async Task<List<Response.TeamNotificationResponse>> GetTeamNotifications(Guid teamId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyTeamRegisterEvents(string? status, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<string> LeaveTeam(Guid teamId) => throw new NotImplementedException();
    public async Task<Response.AppealResponse> AppealRound(Guid teamId, Guid roundId, Request.RoundAppealRequest request) => throw new NotImplementedException();
    public async Task<Response.AppealResponse> AppealSubmission(Guid teamId, Guid submissionId, Request.SubmissionAppealRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetAdminTeams(string? keyword, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<string> DisableTeam(Guid teamId) => throw new NotImplementedException();
    public async Task<string> EnableTeam(Guid teamId) => throw new NotImplementedException();
    public async Task<string> LockTeam(Guid teamId) => throw new NotImplementedException();
    public async Task<string> UnlockTeam(Guid teamId) => throw new NotImplementedException();
}
