using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IUserRepository
{
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(Guid id);
    Task<int> CountByRoleAsync(Domain.Enums.User.RoleEnum? role);
    Task<List<Users>> GetRecentAsync(int count);
    Task<(List<Users> Items, int TotalCount)> SearchAsync(string? keyword, Domain.Enums.User.RoleEnum? role, bool? isDisable, bool? isVerified, int pageIndex, int pageSize);
    Task AddAsync(Users user);
    Task UpdateAsync(Users user);
}
