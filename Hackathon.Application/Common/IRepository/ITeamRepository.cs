using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITeamRepository
{
    Task<int> CountAsync(bool? isDisable);
    Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);
    Task<List<Guid>> GetTeamMemberIdsAsync(Guid teamId);
    Task<Teams?> GetByIdAsync(Guid id);
    Task<List<TeamDetails>> GetTeamMembersAsync(Guid teamId);
    Task UpdateAsync(Teams team);
    Task<(List<Teams> Items, int TotalCount)> SearchAsync(
        string? keyword, bool? canEdit,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
}
