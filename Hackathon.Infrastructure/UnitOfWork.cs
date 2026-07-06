using Hackathon.Application.Common.Interfaces;
using Hackathon.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Hackathon.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new ServerException(ErrorCode5Xx.InternalServerError, ErrorMessage.Database.SaveChangesFailed);
        }
    }
}


