namespace Hackathon.Application.Services.Lecturer.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(GetCriteriaTemplatesByRoundRequest request);
    Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId);
    Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(GetCriteriaItemsByTemplateRequest request);
    Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId);
}
