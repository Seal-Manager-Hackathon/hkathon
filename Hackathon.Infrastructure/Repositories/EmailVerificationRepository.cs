using Hackathon.Application.Common.Interfaces;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class EmailVerificationRepository : IEmailVerificationRepository
{
    private readonly AppDbContext _context;

    public EmailVerificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmailVerifications?> GetByUserIdAsync(Guid userId)
        => await _context.EmailVerifications.FirstOrDefaultAsync(x => x.UserId == userId);

    public async Task<List<EmailVerifications>> GetPendingByUserIdAsync(Guid userId)
        => await _context.EmailVerifications
            .Where(x => x.UserId == userId && x.Status == Domain.Enums.EmailVerification.EmailVerificationStatusEnum.Pending && !x.IsDisable)
            .ToListAsync();

    public async Task AddAsync(EmailVerifications emailVerification)
        => await _context.EmailVerifications.AddAsync(emailVerification);

    public Task UpdateRangeAsync(List<EmailVerifications> emailVerifications)
    {
        _context.EmailVerifications.UpdateRange(emailVerifications);
        return Task.CompletedTask;
    }
}
