using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IEmailVerificationRepository
{
    Task<EmailVerifications?> GetByUserIdAsync(Guid userId);
    Task<List<EmailVerifications>> GetPendingByUserIdAsync(Guid userId);
    Task AddAsync(EmailVerifications emailVerification);
    Task UpdateRangeAsync(List<EmailVerifications> emailVerifications);
}
