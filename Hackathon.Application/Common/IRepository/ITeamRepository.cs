using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITeamRepository
{
    Task<int> CountAsync(bool? isDisable);
}
