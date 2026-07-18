using Hackathon.Application.Common.IRepository;
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

    public async Task<List<Users>> GetRecentAsync(int count)
        => await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(count)
            .ToListAsync();

    public async Task<int> CountByRoleAsync(Domain.Enums.User.RoleEnum? role)
    {
        var query = _context.Users.AsQueryable();
        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);
        return await query.CountAsync();
    }

    public async Task<(List<Users> Items, int TotalCount)> SearchByEmailAsync(
        string? keyword, bool? isDisable, int pageIndex, int pageSize)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(u => u.Email.ToLower().Contains(kw));
        }

        if (isDisable.HasValue)
            query = query.Where(u => u.IsDisable == isDisable.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Users> Items, int TotalCount)> SearchAsync(
        string? keyword, Domain.Enums.User.RoleEnum? role, bool? isDisable, bool? isVerified, bool? isBanned,
        DateTimeOffset? fromDate, DateTimeOffset? toDate,
        int pageIndex, int pageSize)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(u =>
                u.Email.ToLower().Contains(kw) ||
                u.FirstName.ToLower().Contains(kw) ||
                u.LastName.ToLower().Contains(kw) ||
                (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(kw));
        }

        if (role.HasValue)
            query = query.Where(u => u.Role == role.Value);

        if (isDisable.HasValue)
            query = query.Where(u => u.IsDisable == isDisable.Value);

        if (isVerified.HasValue)
            query = query.Where(u => u.IsVerified == isVerified.Value);

        if (isBanned.HasValue)
        {
            if (isBanned.Value)
                query = query.Where(u => u.BanReason != null);
            else
                query = query.Where(u => u.BanReason == null);
        }

        if (fromDate.HasValue)
            query = query.Where(u => u.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(u => u.CreatedAt <= toDate.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Users> Items, int TotalCount)> GetAvailableUsersByRoleAsync(Guid eventId, Domain.Enums.User.RoleEnum role, string? keyword, int pageIndex, int pageSize)
    {
        var query = _context.Users
            .Where(u => u.Role == role)
            .Where(u => !u.IsDisable)
            .Where(u => u.BanReason == null)
            .Where(u => !u.AssignEvents.Any(ae => ae.EventId == eventId && !ae.IsDisable));

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(u =>
                u.Email.ToLower().Contains(kw) ||
                (u.FirstName.ToLower() + " " + u.LastName.ToLower()).Contains(kw));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(u => u.Email)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<Users>> GetAllAsync()
        => await _context.Users.ToListAsync();

    public async Task<Users?> GetByEmailAsync(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

    public async Task<Users?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task<Users?> GetByStudentIdAsync(string studentId)
        => await _context.Users.FirstOrDefaultAsync(x => x.StudentId.ToUpper() == studentId.ToUpper());

    public async Task AddAsync(Users user)
        => await _context.Users.AddAsync(user);

    public Task UpdateAsync(Users user)
    {
        _context.Users.Update(user);
        return Task.CompletedTask;
    }
}
