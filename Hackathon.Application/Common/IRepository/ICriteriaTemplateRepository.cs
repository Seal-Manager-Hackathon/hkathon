using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ICriteriaTemplateRepository
{
    Task<List<CriteriaTemplates>> GetByRoundIdAsync(Guid roundId);
}
