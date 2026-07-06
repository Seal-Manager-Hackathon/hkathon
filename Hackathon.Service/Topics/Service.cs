using Hackathon.Repository;
using Hackathon.Repository.Enum;
using Hackathon.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Topics;

public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext, IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }

    // #{Public} #{Student}
    public async Task<Response.AssignedTopicResponse> GetTopic(Guid eventId, Guid registerTeamId) => throw new NotImplementedException();
    public async Task<Response.TopicDetailResponse> GetTopicDetail(Guid topicId) => throw new NotImplementedException();

    // #{Admin} #{Staff}
    public async Task<Response.CreateTopicResponse> CreateTopic(Guid trackId, Request.CreateTopicRequest request) => throw new NotImplementedException();
    public async Task<string> UpdateTopic(Guid topicId, Request.UpdateTopicRequest request) => throw new NotImplementedException();
    public async Task<string> DeleteTopic(Guid topicId) => throw new NotImplementedException();
}
