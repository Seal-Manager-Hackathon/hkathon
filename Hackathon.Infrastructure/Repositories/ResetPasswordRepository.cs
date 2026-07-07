using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class ResetPasswordRepository : IResetPasswordRepository
{
    private readonly AppDbContext _context;

    public ResetPasswordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResetPasswords?> GetByTokenAsync(string token)
        => await _context.Set<ResetPasswords>()
            .FirstOrDefaultAsync(rp => rp.TokenHash == token && !rp.IsUsed && !rp.IsDisable);

    public async Task AddAsync(ResetPasswords resetPassword)
        => await _context.Set<ResetPasswords>().AddAsync(resetPassword);

    public Task UpdateAsync(ResetPasswords resetPassword)
    {
        _context.Set<ResetPasswords>().Update(resetPassword);
        return Task.CompletedTask;
    }
}
