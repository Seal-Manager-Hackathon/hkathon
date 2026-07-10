using Hackathon.Application.Services.Admin.CriteriaTemplate;

namespace Hackathon.Application.Services.Lecturer.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplatesByRoundResponse> GetCriteriaTemplatesByRound(Guid roundId, string? keyword);
    Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId);
    Task<GetCriteriaItemsByTemplateResponse> GetCriteriaItemsByTemplate(Guid templateId, string? keyword);
    Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId);
}
