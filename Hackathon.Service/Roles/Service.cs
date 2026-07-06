using Hackathon.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Service.Roles;

public class Service : IService
{
    private readonly AppDbContext _dbContext;

    public Service(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // #{Public}
    public async Task<List<RoleResponse>> GetRoles() => throw new NotImplementedException();
    public async Task<List<EventRoleResponse>> GetEventRoles() => throw new NotImplementedException();
}
