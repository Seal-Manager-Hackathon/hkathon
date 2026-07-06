using Hackathon.Application.Common.IRepository;
using Hackathon.Domain.Entities;

namespace Hackathon.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(RefreshTokens refreshToken)
        => await _context.RefreshTokens.AddAsync(refreshToken);
}
