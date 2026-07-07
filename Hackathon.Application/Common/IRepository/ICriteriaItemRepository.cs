using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ICriteriaItemRepository
{
    Task<CriteriaItems?> GetByIdAsync(Guid id);
}