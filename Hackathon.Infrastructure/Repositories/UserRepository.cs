using Hackathon.Application.Common.Interfaces;
using Hackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Users?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

    public async Task<Users?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task AddAsync(Users user)
        => await _context.Users.AddAsync(user);

    public Task UpdateAsync(Users user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }
}
