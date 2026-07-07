using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IAssignEventRepository
{
    Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersByEventAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        int pageIndex, int pageSize);
    Task<AssignEvents?> GetByIdAsync(Guid id);
    Task<AssignEvents?> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId);
    void Add(AssignEvents assignEvent);
    void Update(AssignEvents assignEvent);
    Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName);
    Task<(List<AssignEvents> Items, int TotalCount)> GetAllAssignedUsersAsync(
        Guid eventId, string? keyword, Domain.Enums.EventRole.EventRoleEnum? eventRole,
        int pageIndex, int pageSize);
}
