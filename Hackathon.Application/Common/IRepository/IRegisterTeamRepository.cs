using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.RegisterTeam;

namespace Hackathon.Application.Common.IRepository;

public interface IRegisterTeamRepository
{
    Task<(List<RegisterTeams> Items, int TotalCount)> SearchAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        Guid? roundId, Guid? trackId, Guid? topicId,
        int pageIndex, int pageSize);
    Task<(List<RegisterTeams> Items, int TotalCount)> SearchWithScoresAsync(
        Guid eventId, string? keyword, RegisterTeamStatusEnum? status,
        bool? isBanned, bool? isDisable,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        Guid? roundId, Guid? trackId, Guid? topicId,
        int pageIndex, int pageSize);
    Task<RegisterTeams?> GetByIdAsync(Guid id);
    Task AddAsync(RegisterTeams registerTeam);
    Task UpdateAsync(RegisterTeams registerTeam);
    Task<bool> HasOtherActiveRegistrationAsync(Guid teamId, Guid excludeRegisterTeamId);
    Task<bool> HasAnyMemberApprovedInEventAsync(Guid eventId, List<Guid> userIds);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetApprovedByUserIdAsync(Guid userId, string? keyword, int pageIndex, int pageSize);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetByTeamIdAsync(Guid teamId, RegisterTeamStatusEnum? status, bool? isDisable, int pageIndex, int pageSize);
    Task<RegisterTeams?> GetByIdWithRoundDetailsAsync(Guid id);
    Task<RegisterTeams?> GetByIdWithRoundDetailsAndScoresAsync(Guid id);
    Task<int> CountByTrackIdAsync(Guid trackId);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetApprovedByEventIdWithScoresAsync(Guid eventId, int pageIndex, int pageSize);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetByTrackIdAsync(Guid trackId, string? keyword, int pageIndex, int pageSize);
    Task<(List<RegisterTeams> Items, int TotalCount)> GetByEventIdAndTeamIdAsync(
        Guid eventId, Guid teamId, RegisterTeamStatusEnum? status,
        int pageIndex, int pageSize);
}
