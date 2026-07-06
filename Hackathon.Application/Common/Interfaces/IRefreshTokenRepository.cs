using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshTokens refreshToken);
}
