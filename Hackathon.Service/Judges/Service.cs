using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Judges;

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

    // #{Judge}
    public async Task<List<Response.JudgeTrackResponse>> GetMyTracks() => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetTrackSubmissions(Guid trackId, Guid? roundId, bool? isGraded, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetEventSubmissions(Guid eventId, Guid? trackId, Guid? roundId, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetPendingSubmissions(Guid eventId, Guid? trackId, Guid? roundId, bool? isGraded, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetCurrentEventPendingSubmissions(Guid? trackId, Guid? roundId, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> SearchSubmissions(Guid eventId, Guid? trackId, string? keyword, bool? isGraded, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetRegradeSubmissions(Guid? eventId, Guid? trackId, bool? isRegraded, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.SubmissionCriteriaResponse> GetSubmissionCriteria(Guid submissionId) => throw new NotImplementedException();
    public async Task<Response.JudgeSubmissionScoreResponse?> GetMySubmissionScore(Guid submissionId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetMyScores(Guid eventId, Guid? trackId, bool? isGraded, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<Response.JudgeSubmissionScoreResponse> SubmitScore(Guid submissionId, Request.SubmitScoreRequest request) => throw new NotImplementedException();
    public async Task<Response.JudgeSubmissionScoreResponse> SubmitMockScore(Guid submissionId, Request.SubmitScoreRequest request) => throw new NotImplementedException();
    public async Task<Response.JudgeSubmissionScoreResponse> UpdateScore(Guid scoreId, Request.SubmitScoreRequest request) => throw new NotImplementedException();
    public async Task<Response.JudgeScoreItemResponse> UpdateScoreItem(Guid scoreId, Guid scoreItemId, Request.UpdateScoreItemRequest request) => throw new NotImplementedException();
    public async Task<string> FinalizeScore(Guid scoreId) => throw new NotImplementedException();
    public async Task<Response.JudgeSubmissionScoreResponse> SubmitRetakeScore(Guid scoreId, Request.SubmitScoreRequest request) => throw new NotImplementedException();
    public async Task<(List<Response.JudgeTrackTeamResponse> Data, string Message)> GetJudgeTeamsByEvent(Guid eventId, Guid? roundId) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetJudgeRoundTeams(Guid eventId, Guid roundId, Guid? trackId, string? status, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetJudgeTeamSubmissions(Guid registerTeamId, PaginationRequest paginationRequest) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetJudgeRoundAllSubmissions(Guid roundId, string? status, PaginationRequest paginationRequest) => throw new NotImplementedException();
}
