using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Common.IRepository;
using Hackathon.Application.Exceptions;
using Hackathon.Domain.Enums.User;
using ErrMsg = Hackathon.Application.Exceptions.ErrorMessage;

namespace Hackathon.Application.Services.Staff.CriteriaTemplate;

public interface ICriteriaTemplateService
{
    Task<GetCriteriaTemplateResponse> GetCriteriaTemplateByRoundId(Guid roundId);
    Task<GetCriteriaItemsResponse> GetCriteriaItemsByTemplateId(Guid criteriaTemplateId);
    Task<GetCriteriaTemplateDetailResponse> GetCriteriaTemplateDetail(Guid templateId);
    Task<GetCriteriaItemDetailResponse> GetCriteriaItemDetail(Guid itemId);
}
