using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Rounds;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IRoundEndScheduler _roundEndScheduler;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext, IRoundEndScheduler roundEndScheduler)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
        _roundEndScheduler = roundEndScheduler;
    }

    private Guid GetCurrentUserId() => throw new NotImplementedException();
    private bool IsCurrentUserAdmin() => throw new NotImplementedException();
    private async Task EnsureStaffAssignedToEvent(Guid eventId) => throw new NotImplementedException();

    // #{Public} #{Student}
    public async Task<List<Response.RoundResponse>> GetRounds(Guid eventId) => throw new NotImplementedException();
    public async Task<Response.RoundDetailResponse> GetRound(Guid roundId) => throw new NotImplementedException();
    public async Task<List<Response.MyRoundResponse>> GetMyRounds(Guid? eventId, Guid teamId) => throw new NotImplementedException();
    public async Task<Response.MyRoundDetailResponse> GetMyRoundDetail(Guid registerTeamId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetRoundRanking(Guid roundId, Request.GetSubmissionsQuery query) => throw new NotImplementedException();
    public async Task<Response.MyRoundScoreResponse> GetMyRoundScore(Guid roundId) => throw new NotImplementedException();
    public async Task<Response.TeamLatestSubmissionScoreResponse> GetTeamLatestSubmissionScore(Guid roundId, Guid teamId) => throw new NotImplementedException();
    public async Task<Response.CreateSubmissionResponse> CreateSubmission(Guid roundId, Request.CreateSubmissionRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query) => throw new NotImplementedException();

    // #{Staff}
    public async Task<BasePaginationResponse> GetStaffRoundSubmissions(Guid roundId, Request.GetStaffRoundSubmissionsQuery query) => throw new NotImplementedException();
    public async Task<Response.AssignJudgesToSubmissionResponse> AssignJudgesToSubmission(Guid submissionId, Request.AssignJudgesToSubmissionRequest request) => throw new NotImplementedException();

    // #{Admin} #{Staff}
    public async Task<Response.EndRoundResponse> EndRound(Guid roundId) => throw new NotImplementedException();

    // #{Admin}
    public async Task<string> EndRoundFinal(Guid roundId) => throw new NotImplementedException();
    public async Task UpdateRound(Guid roundId, Request.UpdateRoundRequest request) => throw new NotImplementedException();

    // #{Lecturer}
    public async Task<BasePaginationResponse> GetLecturerRoundSubmissions(Guid roundId, Request.GetSubmissionsQuery query) => throw new NotImplementedException();

    // Used by EndRoundJob (background job)
    public static async Task CloseAndAdvanceRoundAsync(AppDbContext dbContext, Repository.Entity.Rounds round, DateTimeOffset now) => throw new NotImplementedException();
}
