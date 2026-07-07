using Hackathon.Domain.Entities;

namespace Hackathon.Application.Common.IRepository;

public interface IAssignEventRepository
{
    Task<(List<AssignEvents> Items, int TotalCount)> GetAssignedUsersAsync(
        Guid eventId, string? keyword, Domain.Enums.User.RoleEnum? role,
        int pageIndex, int pageSize);
    Task<AssignEvents?> GetByEventIdAndUserIdAsync(Guid eventId, Guid userId);
    void Add(AssignEvents assignEvent);
    Task<EventRoles?> GetEventRoleByNameAsync(Domain.Enums.EventRole.EventRoleEnum roleName);
}
