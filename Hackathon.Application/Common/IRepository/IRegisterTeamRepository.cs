using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;

namespace Hackathon.Application.Common.IRepository;

public interface IRegisterTeamRepository
{
    Task<(List<RegisterTeams> Items, int TotalCount)> SearchAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
    Task<RegisterTeams?> GetByIdAsync(Guid id);
    Task UpdateAsync(RegisterTeams registerTeam);
    Task<bool> HasOtherApprovedAsync(Guid teamId, Guid excludeRegisterTeamId);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetApprovedByUserIdAsync(Guid userId, string? keyword, int pageIndex, int pageSize);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetByTeamIdAsync(Guid teamId, RegisterTeamStatusEnum? status, bool? isDisable, int pageIndex, int pageSize);
    Task<int> CountByTrackIdAsync(Guid trackId);
}
