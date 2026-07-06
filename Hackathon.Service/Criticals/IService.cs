namespace Hackathon.Service.Criticals;

public interface IService
{
    // #{Public} #{Student}
    Task<List<Response.RoundCriteriaResponse>> GetCriteriaByRound(Guid roundId);
    Task<List<Response.RoundCriteriaResponse>> GetCriteriaByEvent(Guid eventId);

    // #{Admin}
    Task<Response.CreateCriteriaResponse> CreateCriteria(Guid eventId, Guid roundId, Request.CreateCriteriaRequest request);
    Task ActivateCriteria(Guid eventId, Guid roundId, Guid templateId);
    Task<List<Response.CriteriaTemplateResponse>> GetCriteriaTemplatesByRound(Guid eventId, Guid roundId);
}
