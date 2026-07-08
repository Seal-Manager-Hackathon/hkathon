namespace Hackathon.Application.Services.Admin.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(GetCriteriaTemplatesByRoundRequest request);
    Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId);
    Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(GetCriteriaItemsByTemplateRequest request);
    Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId);
    Task CreateCriteriaTemplate(CreateCriteriaTemplateRequest request);
    Task UpdateCriteriaTemplate(UpdateCriteriaTemplateRequest request);
    Task DeleteCriteriaTemplate(Guid templateId);
    Task RestoreCriteriaTemplate(Guid templateId);
    Task ActivateCriteriaTemplate(Guid templateId);
    Task CreateCriteriaItem(Guid templateId, CreateCriteriaItemRequest request);
    Task UpdateCriteriaItem(UpdateCriteriaItemRequest request);
    Task DeleteCriteriaItem(Guid itemId);
    Task RestoreCriteriaItem(Guid itemId);
}
