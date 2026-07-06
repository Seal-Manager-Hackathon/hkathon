using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Hackathon.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Submissions;

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

    // #{Public} #{Student}
    public async Task<Response.SubmissionDetailResponse> GetSubmissionDetail(Guid submissionId) => throw new NotImplementedException();
    public async Task<Response.SubmitRoundProjectResponse> SubmitRoundProject(Guid roundId, Guid registerTeamId, Request.SubmitRoundProjectRequest request) => throw new NotImplementedException();
    public async Task<BasePaginationResponse> GetSubmissions(Guid roundId, Guid registerTeamId, Request.GetSubmissionsRequest request) => throw new NotImplementedException();
}
