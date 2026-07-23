using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IUserRepository
{
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(Guid id);
    Task<Users?> GetByStudentIdAsync(string studentId);
    Task<int> CountByRoleAsync(Domain.Enums.User.RoleEnum? role);
    Task<List<Users>> GetRecentAsync(int count);
    Task<(List<Users> Items, int TotalCount)> SearchAsync(string? keyword, Domain.Enums.User.RoleEnum? role, bool? isDisable, bool? isVerified, bool? isBanned, DateTimeOffset? fromDate, DateTimeOffset? toDate, int pageIndex, int pageSize);
    Task<(List<Users> Items, int TotalCount)> GetAvailableUsersByRoleAsync(Guid eventId, Domain.Enums.User.RoleEnum role, string? keyword, int pageIndex, int pageSize);
    Task<List<Users>> GetAllAsync();
    Task AddAsync(Users user);
    Task<List<string>> GetEmailsByPrefixAsync(string prefix, string domain);
    Task<List<string>> GetStudentIdsByPrefixAsync(string prefix);
    Task<List<string>> GetPhoneNumbersByPrefixAsync(string prefix);
    Task<(List<Users> Items, int TotalCount)> SearchByEmailAsync(string? keyword, bool? isDisable, int pageIndex, int pageSize);
    Task UpdateAsync(Users user);
}
