using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Invitation;

namespace Hackathon.Application.Common.IRepository;

public interface IInvitationRepository
{
    Task<(List<Invitations> Items, int TotalCount)> GetByTeamIdAsync(
        Guid teamId, string? keyword, InvitationStatusEnum? status,
        int pageIndex, int pageSize);
}
