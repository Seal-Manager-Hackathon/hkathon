namespace Hackathon.Service.Roles;

public class RoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public class EventRoleResponse
{
    public Guid RoleId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
