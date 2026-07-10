using Hackathon.Domain.Entities;
using Hackathon.Domain.Enums.Event;

namespace Hackathon.Application.Common.IRepository;

public interface IAssignEventRepository
{
    Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersByEventAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        int pageIndex, int pageSize);
    Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersByEventAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        Domain.Enums.User.RoleEnum? role, Guid? trackId,
        int pageIndex, int pageSize);
    Task<AssignEvents?> GetByIdAsync(Guid id);
    Task<AssignEvents?> GetByIdWithTracksAsync(Guid id);
    Task<AssignEvents?> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId);
    Task<bool> IsTrackAssignedAsync(Guid assignEventId, Guid trackId);
    void Add(AssignEvents assignEvent);
    void Update(AssignEvents assignEvent);
    void AddAssignTrack(AssignTracks assignTrack);
    Task<AssignTracks?> GetAssignTrackAsync(Guid assignEventId, Guid trackId);
    Task<AssignTracks?> GetAssignTrackAnyAsync(Guid assignEventId, Guid trackId);
    Task<AssignTracks?> GetGraderAssignTrackAsync(Guid userId, Guid eventId, Guid trackId);
    void RemoveAssignTrack(AssignTracks assignTrack);
    void RestoreAssignTrack(AssignTracks assignTrack);
    Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName);
    Task<(List<AssignEvents> Items, int TotalCount)> GetEventsByStaffUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
    Task<(List<AssignEvents> Items, int TotalCount)> GetStaffAssignEventsByUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
    Task<List<AssignEvents>> GetCurrentAssignedEventsByUserIdAsync(Guid userId);
    Task<AssignEvents?> GetByEventIdAndUserIdWithEventAsync(Guid eventId, Guid userId);
    Task<(List<AssignEvents> Items, int TotalCount)> GetLecturerAssignEventsByUserIdAsync(
        Guid userId, string? keyword, EventStatusEnum? status,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize);
    Task<AssignEvents?> GetByEventIdAndUserIdWithTracksAsync(Guid eventId, Guid userId);
}
