using Hackathon.Repository;
using Hackathon.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Criticals;

public class Service : IService
{
    private readonly AppDbContext _dbContext;

    public Service(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // #{Public} #{Student}
    public async Task<List<Response.RoundCriteriaResponse>> GetCriteriaByRound(Guid roundId) => throw new NotImplementedException();
    public async Task<List<Response.RoundCriteriaResponse>> GetCriteriaByEvent(Guid eventId) => throw new NotImplementedException();

    // #{Admin}
    public async Task<Response.CreateCriteriaResponse> CreateCriteria(Guid eventId, Guid roundId, Request.CreateCriteriaRequest request) => throw new NotImplementedException();
    public async Task ActivateCriteria(Guid eventId, Guid roundId, Guid templateId) => throw new NotImplementedException();
    public async Task<List<Response.CriteriaTemplateResponse>> GetCriteriaTemplatesByRound(Guid eventId, Guid roundId) => throw new NotImplementedException();
}
