using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(Guid id);
    Task AddAsync(Users user);
    Task UpdateAsync(Users user);
}
