using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;

namespace Hackathon.Application.Common.IRepository;

public interface IInvitationRepository
{
    Task<(List<Invitations> Items, int TotalCount)> GetByTeamIdAsync(
        Guid teamId, string? keyword, InvitationStatusEnum? status,
        int pageIndex, int pageSize);
    Task AddAsync(Invitations invitation);
    Task<Invitations?> GetByIdAsync(Guid id);
    Task<(List<Invitations> Items, int TotalCount)> GetByUserIdAsync(
        Guid userId, string? keyword, InvitationStatusEnum? status,
        int pageIndex, int pageSize);
}
