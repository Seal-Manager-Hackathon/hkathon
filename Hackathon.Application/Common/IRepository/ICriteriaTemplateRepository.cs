using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ICriteriaTemplateRepository
{
    Task<CriteriaTemplates?> GetByIdAsync(Guid id);
    Task<List<CriteriaTemplates>> GetByRoundIdAsync(Guid roundId);
    Task<List<CriteriaItems>> GetItemsByTemplateIdAsync(Guid templateId);
    Task AddAsync(CriteriaTemplates template);
}
