using Hackathon.Application.Services.Admin.CriteriaTemplate;

namespace Hackathon.Application.Services.Base.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(Guid roundId);
}
