using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IUserRepository
{
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(Guid id);
    Task<int> CountByRoleAsync(Domain.Enums.User.RoleEnum? role);
    Task<List<Users>> GetRecentAsync(int count);
    Task AddAsync(Users user);
    Task UpdateAsync(Users user);
}
