using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface ITeamRepository
{
    Task<int> CountAsync(bool? isDisable);
    Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);
    Task<List<Guid>> GetUserTeamIdsAsync(Guid userId);
    Task<List<Guid>> GetTeamMemberIdsAsync(Guid teamId);
    Task<Teams?> GetByIdAsync(Guid id);
    Task<List<TeamDetails>> GetTeamMembersAsync(Guid teamId);
    Task<(List<TeamDetails> Items, int TotalCount)> GetTeamMembersPagedAsync(Guid teamId, int pageIndex, int pageSize);
    Task UpdateAsync(Teams team);
    Task<(List<TeamDetails> Items, int TotalCount)> GetUserTeamsAsync(Guid userId, string? keyword, Domain.Enums.TeamDetail.TeamDetailStatusEnum? status, bool? isDisable, int pageIndex, int pageSize);
    Task<(List<Teams> Items, int TotalCount)> SearchAsync(
        string? keyword, bool? canEdit,
        DateTimeOffset? fromDate, DateTimeOffset? toDate, bool? isDisable,
        int pageIndex, int pageSize);
    Task<List<Teams>> GetRecentAsync(int count);
    Task AddAsync(Teams team);
    Task AddTeamDetailAsync(TeamDetails teamDetail);
}
