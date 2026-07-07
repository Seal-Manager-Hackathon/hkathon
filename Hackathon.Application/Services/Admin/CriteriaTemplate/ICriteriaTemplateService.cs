namespace Hackathon.Application.Services.Admin.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(GetCriteriaTemplatesByRoundRequest request);
    Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(GetCriteriaItemsByTemplateRequest request);
    Task CreateCriteriaTemplate(CreateCriteriaTemplateRequest request);
    Task UpdateCriteriaTemplate(UpdateCriteriaTemplateRequest request);
    Task DeleteCriteriaTemplate(Guid templateId);
    Task RestoreCriteriaTemplate(Guid templateId);
}
