using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.RegisterTeams;

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

    // #{Student}
    public async Task<(Response.RegisterTeamActionResponse Data, string Message)> RegisterEvent(Request.RegisterEventRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyRegisteredEvents(Request.GetMyRegisteredEventsRequest request, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamDetailForStudentResponse> GetRegisterTeamDetailForStudent(Guid registerId) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamRejectionReasonResponse> GetRejectionReason(Guid registerId) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamAssignmentStatusResponse> GetRegisterTeamAssignmentStatus(Guid registerTeamId) => throw new NotImplementedException();

    // #{Staff} #{Admin}
    public async Task<BasePaginationResponse> GetRegisterTeamsByEvent(Guid eventId, string? keyword, RegisterTeamStatusEnum? status, bool? isDisable, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamDetailResponse> GetRegisterTeamDetail(Guid registerTeamId) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamActionResponse> AcceptRegisterTeam(Guid registerTeamId) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamActionResponse> RejectRegisterTeam(Guid registerTeamId, Request.RejectRegisterTeamRequest request) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamActionResponse> BanRegisterTeam(Guid registerTeamId, Request.BanTeamRequest request) => throw new NotImplementedException();
    public async Task<Response.RegisterTeamActionResponse> UnbanRegisterTeam(Guid registerTeamId) => throw new NotImplementedException();

    // #{Staff} #{Lecturer} #{Admin}
    public async Task<(List<Response.RegisterTeamTrackResponse> Data, string Message)> GetTeamsByTrack(Guid eventId, Guid trackId, Request.GetTeamsByTrackRequest request) => throw new NotImplementedException();
    public async Task<(List<Response.RegisterTeamApprovedResponse> Data, string Message)> GetApprovedTeams(Guid eventId, Request.GetApprovedTeamsRequest request) => throw new NotImplementedException();
    public async Task<(List<Response.RegisterTeamByRoundResponse> Data, string Message)> GetTeamsByRound(Guid eventId, Request.GetTeamsByRoundRequest request) => throw new NotImplementedException();
    public async Task<Response.TeamRoundSubmissionResponse> GetTeamRoundSubmissions(Guid registerTeamId, Guid? roundId) => throw new NotImplementedException();
}
