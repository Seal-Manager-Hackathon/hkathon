using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IResetPasswordRepository
{
    Task<ResetPasswords?> GetByTokenAsync(string token);
    Task AddAsync(ResetPasswords resetPassword);
    Task UpdateAsync(ResetPasswords resetPassword);
}
