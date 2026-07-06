namespace Hackathon.Service.Roles;

public interface IService
{
    // #{Public}
    Task<List<RoleResponse>> GetRoles();
    Task<List<EventRoleResponse>> GetEventRoles();
}
