using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshTokens refreshToken);
}
